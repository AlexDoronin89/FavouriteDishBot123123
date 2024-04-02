using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FavouriteDishBot.Bot.Router;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace FavouriteDishBot.Bot;

public class BotRequestHandlers
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();

    private ChatsRouter _chatsRouter;

    public BotRequestHandlers()
    {
        _chatsRouter = new ChatsRouter();
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        long chatId = 0;
        int messageFromUserId = 0;
        string textData = "";
        bool canRoute = false;

        try
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    if (update.Message != null)
                    {
                        canRoute = true;
                        chatId = update.Message.Chat.Id;
                        messageFromUserId = update.Message.MessageId;
                        textData = update.Message.Text;
                    }

                    break;

                case UpdateType.CallbackQuery:
                    if (update.CallbackQuery != null)
                    {
                        canRoute = true;
                        chatId = update.CallbackQuery.Message.Chat.Id;
                        messageFromUserId = update.CallbackQuery.Message.MessageId;
                        textData = update.CallbackQuery.Data;
                    }

                    break;
            }

            Logger.Info(
                $"ВХОДЯЩЕЕ СОООБЩЕНИЕ UpdateType = {update.Type}; chatId = {chatId}; messageId = {messageFromUserId}; text = {textData} canRoute = {canRoute}");

            if (canRoute)
            {
                BotMessage botMessage = await Task.Run(() => _chatsRouter.Route(chatId, textData), cancellationToken);

                if (botMessage.HideReplyKeyboard == true)
                {
                    Message message = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: ".",
                        replyMarkup: new ReplyKeyboardRemove(),
                        cancellationToken: cancellationToken);
                    
                    await botClient.DeleteMessageAsync(
                        chatId: chatId,
                        messageId: message.MessageId,
                        cancellationToken: cancellationToken
                    );
                }

                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: botMessage.Text,
                    replyMarkup: botMessage.KeyboardMarkup,
                    cancellationToken: cancellationToken);

                Logger.Info(
                    $"ИСХОДЯЩЕЕ СОООБЩЕНИЕ chatId = {chatId}; text = {botMessage.Text.Replace("\n", " ")}; Keyboard = {getKeyboardAsString(botMessage.KeyboardMarkup)}");
            }
        }
        catch (Exception e)
        {
            await botClient.DeleteMessageAsync(
                chatId: chatId,
                messageId: messageFromUserId,
                cancellationToken: cancellationToken
            );

            Logger.Error($"ОШИБКА chatId = {chatId}; text = {e.Message}");
        }
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Logger.Error($"Ошибка поймана в методе HandlePollingErrorAsync, {errorMessage}");
        return Task.CompletedTask;
    }

    private string getKeyboardAsString(IReplyMarkup keyboardMarkup)
    {
        if (keyboardMarkup == null)
        {
            return "Клавиатуры в данном сообщении нет";
        }

        StringBuilder stringBuilder = new StringBuilder();

        if (keyboardMarkup is InlineKeyboardMarkup)
        {
            InlineKeyboardMarkup inlineKeyboardMarkup = keyboardMarkup as InlineKeyboardMarkup;

            foreach (var row in inlineKeyboardMarkup.InlineKeyboard)
            {
                stringBuilder.Append(row.ToList()[0].Text + ";");
            }
        }
        else if (keyboardMarkup is ReplyKeyboardMarkup)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = keyboardMarkup as ReplyKeyboardMarkup;

            foreach (var row in replyKeyboardMarkup.Keyboard)
            {
                stringBuilder.Append(row.ToList()[0].Text + ";");
            }
        }

        return stringBuilder.ToString();
    }
}