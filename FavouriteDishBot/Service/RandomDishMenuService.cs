using FavouriteDishBot.Bot;
using FavouriteDishBot.Bot.Router;
using FavouriteDishBot.Db.DbConnector;
using FavouriteDishBot.Db.Models;
using FavouriteDishBot.Db.Repositories.Implemintations;
using FavouriteDishBot.Db.Repositories.Interfaces;
using FavouriteDishBot.Util.Button;
using FavouriteDishBot.Util.String;
using Telegram.Bot.Types.ReplyMarkups;

namespace FavouriteDishBot.Service;
public class RandomDishMenuService
{
    private IDishesRepository _dishesRepository;
    private IDishesListsRepository _dishesListsRepository;

    public RandomDishMenuService(IDishesRepository dishesRepository, IDishesListsRepository dishesListsRepository)
    {
        _dishesRepository = dishesRepository;
        _dishesListsRepository = dishesListsRepository;
    }

    public BotMessage ProcessClickOnInlineButtonInRandomDishMenu(string textData, TransmittedData transmittedData)
    {
        List<DishesList> dishesLists = _dishesListsRepository.GetDishesListsByChatId(transmittedData.ChatId);

        if (textData == BotButtonsStorage.RandomDish.AddToDishList.CallBackData)
        {
            transmittedData.State = States.RandomDishMenu.ClickOnReplyButtonUserLists;

            return new BotMessage(DialogsStringsStorage.MyLists, ReplyKeyboardMarkupStorage.CreateKeyboardDishesLists(dishesLists));
        }
        if (textData == BotButtonsStorage.System.ButtonBack.CallBackData)
        {
            transmittedData.State = States.MainMenu.ClickOnInlineButtonInMenuMain;

            return new BotMessage(DialogsStringsStorage.MainMenu, InlineKeyboardMarkupStorage.MainMenuChoose, true);
        }

        throw new Exception("Неизвестная ошибка в ProcessClickOnInlineButtonInRandomDishMenu");
    }

    public BotMessage ProcessClickOnReplyButtonUserLists(string textData, TransmittedData transmittedData)
    {
        if (textData == ConstraintStringsStorage.Back)
        {
            transmittedData.State = States.MainMenu.ClickOnInlineButtonInMenuMain;

            return new BotMessage(DialogsStringsStorage.MainMenu, InlineKeyboardMarkupStorage.MainMenuChoose, true);
        }

        DishesList dishesList = _dishesListsRepository.GetDishesListByTitle(textData, transmittedData.ChatId);

        if (dishesList == null)
        {
            List<DishesList> dishesLists = _dishesListsRepository.GetDishesListsByChatId(transmittedData.ChatId);
            return new BotMessage(DialogsStringsStorage.PressButton,
                ReplyKeyboardMarkupStorage.CreateKeyboardDishesLists(dishesLists));
        }

        transmittedData.State = States.RandomDishMenu.InputRating;
        transmittedData.DataStorage.AddOrUpdate("listId", dishesList.Id);

        return new BotMessage(DialogsStringsStorage.DishRatingInput);

        throw new Exception("Неизвестная ошибка в методе ProcessClickOnInlineButtonUserLists");
    }

    public BotMessage ProcessInputRating(string textData, TransmittedData transmittedData)
    {
        if (int.TryParse(textData, out int inputRating) == false)
        {
            return new BotMessage(DialogsStringsStorage.DishRatingErrorInput);
        }
        if (inputRating < ConstraintStringsStorage.DishMinRating || inputRating > ConstraintStringsStorage.DishMaxRating)
        {
            return new BotMessage(DialogsStringsStorage.DishRatingErrorInput);
        }

        transmittedData.State = States.RandomDishMenu.InputComment;
        transmittedData.DataStorage.AddOrUpdate("dishRating", inputRating);

        return new BotMessage(DialogsStringsStorage.DishCommentInput);

        throw new Exception("Неизвестная ошибка в методе ProcessInputRating");
    }

    public BotMessage ProcessInputComment(string textData, TransmittedData transmittedData)
    {
        if (textData.Length > ConstraintStringsStorage.DishCommentMaxLength)
        {
            return new BotMessage(DialogsStringsStorage.DishCommentInputError);
        }

        transmittedData.State = States.MainMenu.ClickOnInlineButtonInMenuMain;

        Db.Models.Dish dish = new Db.Models.Dish();

        dish.Title = (string)transmittedData.DataStorage.Get("dishTitle");
        dish.Ingredients = (string)transmittedData.DataStorage.Get("dishIngredients");
        dish.Recipe = (string)transmittedData.DataStorage.Get("dishRecipe");
        dish.Rating = (int)transmittedData.DataStorage.Get("dishRating");
        dish.Comment = textData;

        DishesList dishesList = _dishesListsRepository.GetDishesListById((int)transmittedData.DataStorage.Get("listId"));
        _dishesListsRepository.AddDishInDishesList(dishesList, dish);

        return new BotMessage(DialogsStringsStorage.DishAddedToList(dish, dishesList)+ "\n\n" + DialogsStringsStorage.MainMenu, InlineKeyboardMarkupStorage.MainMenuChoose);

        throw new Exception("Неизвестная ошибка в методе ProcessInputComment");
    }
}