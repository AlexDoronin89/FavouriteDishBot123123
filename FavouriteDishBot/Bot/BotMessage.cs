using Telegram.Bot.Types.ReplyMarkups;

namespace FavouriteDishBot.Bot;

public class BotMessage
{
    public string Text { get; }
    public IReplyMarkup KeyboardMarkup { get; }
    public bool HideReplyKeyboard { get; }

    public BotMessage(string text)
    {
        Text = text;
        KeyboardMarkup = null;
        HideReplyKeyboard = false;
    }

    public BotMessage(string text, IReplyMarkup keyboardMarkup)
    {
        Text = text;
        KeyboardMarkup = keyboardMarkup;
        HideReplyKeyboard = false;
    }
    public BotMessage(string text, IReplyMarkup keyboardMarkup, bool hideReplyKeyboard)
    {
        Text = text;
        KeyboardMarkup = keyboardMarkup;
        HideReplyKeyboard = hideReplyKeyboard;
    }
}