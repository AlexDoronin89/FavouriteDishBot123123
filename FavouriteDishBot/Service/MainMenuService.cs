using FavouriteDishBot.Bot;
using FavouriteDishBot.Bot.Router;
using FavouriteDishBot.Db.DbConnector;
using FavouriteDishBot.Db.Models;
using FavouriteDishBot.Db.Repositories.Interfaces;
using FavouriteDishBot.Util.Button;
using FavouriteDishBot.Util.String;
using Telegram.Bot.Types.ReplyMarkups;
using FavouriteDishBot.Parser;

namespace FavouriteDishBot.Service;

public class MainMenuService
{
    private IDishesListsRepository _dishesListsRepository;
    private DishParser _dishesParser;

    public MainMenuService(IDishesListsRepository dishesListsRepository)
    {
        _dishesListsRepository = dishesListsRepository;
        _dishesParser = new DishParser();
    }

    public BotMessage ProcessClickOnInlineButton(string textData, TransmittedData transmittedData)
    {
        List<DishesList> dishesLists = _dishesListsRepository.GetDishesListsByChatId(transmittedData.ChatId);
       
        if (textData == ConstraintStringsStorage.CreateDishList)
        {
            transmittedData.State = States.ListMenu.InputListName;
            return new BotMessage(DialogsStringsStorage.ListNameInput);
        }
        if (textData == ConstraintStringsStorage.Lists)
        {
            if (dishesLists.Count == 0)
            {
                return new BotMessage(DialogsStringsStorage.NoLists + DialogsStringsStorage.MainMenu,
                    InlineKeyboardMarkupStorage.MainMenuChoose);
            }

            transmittedData.State = States.ListMenu.ClickOnReplyButtonUserLists;

            return new BotMessage(DialogsStringsStorage.MyLists, ReplyKeyboardMarkupStorage.CreateKeyboardDishesLists(dishesLists));
        }
        if (textData == ConstraintStringsStorage.RandomDish)
        {
            Dish randomDish=_dishesParser.GetRandomDish();

            transmittedData.DataStorage.AddOrUpdate("dishTitle", randomDish.Title);
            transmittedData.DataStorage.AddOrUpdate("dishIngredients", randomDish.Ingredients);
            transmittedData.DataStorage.AddOrUpdate("dishRecipe", randomDish.Recipe);

            transmittedData.State = States.RandomDishMenu.ClickOnInlineButtonInRandomDishMenu;

            return new BotMessage(DialogsStringsStorage.RandomDishParameter(randomDish), InlineKeyboardMarkupStorage.RandomDish);
        }
       

        throw new Exception("Неизвестная ошибка в ProcessClickOnInlineButton");
    }
}