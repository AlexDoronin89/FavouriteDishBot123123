using System;
using FavouriteDishBot.Bot;
using FavouriteDishBot.Bot.Router;
using FavouriteDishBot.Util;
using FavouriteDishBot.Util.Button;
using FavouriteDishBot.Util.String;

namespace FavouriteDishBot.Service;

public class StartMenuService
{
    public BotMessage ProcessCommandStart(string textData, TransmittedData transmittedData)
    {
        if (textData != SystemStringsStorage.CommandStart)
        {
            return new BotMessage(DialogsStringsStorage.CommandStartInputErrorInput);
        }

        transmittedData.State = States.MainMenu.ClickOnInlineButtonInMenuMain;

        return new BotMessage(DialogsStringsStorage.MainMenu, InlineKeyboardMarkupStorage.MainMenuChoose);
    }
}