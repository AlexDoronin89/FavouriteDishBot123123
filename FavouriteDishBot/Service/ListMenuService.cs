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

public class ListMenuService
{
    private IDishesRepository _dishesRepository;
    private IDishesListsRepository _dishesListsRepository;

    public ListMenuService(IDishesRepository dishesRepository, IDishesListsRepository dishesListsRepository)
    {
        _dishesRepository = dishesRepository;
        _dishesListsRepository = dishesListsRepository;
    }

    public BotMessage ProcessInputListName(string textData, TransmittedData transmittedData)
    {
        if (textData.Length > ConstraintStringsStorage.ListNameMaxLength)
        {
            return new BotMessage(DialogsStringsStorage.ListNameInputError);
        }

        transmittedData.State = States.MainMenu.ClickOnInlineButtonInMenuMain;
        _dishesListsRepository.AddDishesList(transmittedData.ChatId, textData);

        return new BotMessage(DialogsStringsStorage.MainMenu, InlineKeyboardMarkupStorage.MainMenuChoose);
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

        transmittedData.State = States.ListMenu.ClickActionButtonWithList;
        transmittedData.DataStorage.AddOrUpdate("listId", dishesList.Id);

        return new BotMessage(DialogsStringsStorage.ChoosedList(dishesList), InlineKeyboardMarkupStorage.ListMenuChoose,
            true);
    }

    public BotMessage ProcessNewListName(string textData, TransmittedData transmittedData)
    {
        if (textData.Length > ConstraintStringsStorage.ListNameMaxLength)
        {
            return new BotMessage(DialogsStringsStorage.ListNameInputError, null);
        }

        DishesList dishesList = _dishesListsRepository.GetDishesListById((int)transmittedData.DataStorage.Get("listId"));

        transmittedData.State = States.ListMenu.ClickActionButtonWithList;

        _dishesListsRepository.UpdateDishesListTitle(dishesList.Id, textData);

        return new BotMessage(DialogsStringsStorage.ListNameChangeSuccess, InlineKeyboardMarkupStorage.ListMenuChoose);
    }

    public BotMessage ProcessClickOnDeleteListButton(string textData, TransmittedData transmittedData)
    {
        DishesList dishesList = _dishesListsRepository.GetDishesListById((int)transmittedData.DataStorage.Get("listId"));

        if (textData == BotButtonsStorage.DishMenu.Confirm.CallBackData)
        {
            _dishesListsRepository.DeleteDishesList((int)transmittedData.DataStorage.Get("listId"));

            transmittedData.State = States.MainMenu.ClickOnInlineButtonInMenuMain;

            return new BotMessage(DialogsStringsStorage.MainMenu, InlineKeyboardMarkupStorage.MainMenuChoose);
        }

        if (textData == BotButtonsStorage.DishMenu.Cancel.CallBackData)
        {
            transmittedData.State = States.ListMenu.ClickActionButtonWithList;

            return new BotMessage(DialogsStringsStorage.ChoosedList(dishesList),
                InlineKeyboardMarkupStorage.ListMenuChoose);
        }

        throw new Exception("Неизвестная ошибка в ProcessClickOnDeleteListButton");
    }

    public BotMessage ProcessClickActionButtonWithList(string textData, TransmittedData transmittedData)
    {
        DishesList dishesList = _dishesListsRepository.GetDishesListById((int)transmittedData.DataStorage.Get("listId"));

        if (textData == BotButtonsStorage.ListMenu.AddDish.CallBackData)
        {
            transmittedData.State = States.DishMenu.InputTitle;
            return new BotMessage(DialogsStringsStorage.DishTitleInput, null);
        }
        if (textData == BotButtonsStorage.ListMenu.CheckDishes.CallBackData)
        {
            if (((List<Dish>)dishesList.Dishes).Count == 0)
                return new BotMessage(
                    DialogsStringsStorage.DishAreNull + "\n\n" + DialogsStringsStorage.ChoosedList(dishesList),
                    InlineKeyboardMarkupStorage.ListMenuChoose);

            transmittedData.State = States.DishMenu.ClickOnReplyButtonListDishes;
            return new BotMessage(DialogsStringsStorage.DishesInList,
                ReplyKeyboardMarkupStorage.CreateKeyboardDishes((List<Dish>)dishesList.Dishes));
        }
        if (textData == BotButtonsStorage.ListMenu.RenameList.CallBackData)
        {
            transmittedData.State = States.ListMenu.NewListName;

            return new BotMessage(DialogsStringsStorage.NewListNameInput, null);
        }

        if (textData == BotButtonsStorage.ListMenu.DeleteList.CallBackData)
        {
            transmittedData.State = States.ListMenu.ListInputDeletingConfirmation;

            return new BotMessage(DialogsStringsStorage.ListDeletedConfirmation,
                InlineKeyboardMarkupStorage.DishConfirmation);
        }

        if (textData == BotButtonsStorage.ListMenu.BackToLists.CallBackData)
        {
            transmittedData.State = States.ListMenu.ClickOnReplyButtonUserLists;
            transmittedData.DataStorage.Delete("listId");
            List<DishesList> gamesLists = _dishesListsRepository.GetDishesListsByChatId(transmittedData.ChatId);

            return new BotMessage(DialogsStringsStorage.MyLists,
                ReplyKeyboardMarkupStorage.CreateKeyboardDishesLists(gamesLists));
        }

        throw new Exception("Неизвестная ошибка в ProcessClickActionButtonWithList");
    }
}