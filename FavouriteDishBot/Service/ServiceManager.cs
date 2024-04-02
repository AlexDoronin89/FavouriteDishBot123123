using System;
using System.Collections.Generic;
using FavouriteDishBot.Bot;
using FavouriteDishBot.Bot.Router;
using FavouriteDishBot.Db.DbConnector;
using FavouriteDishBot.Db.Repositories.Implemintations;
using FavouriteDishBot.Db.Repositories.Interfaces;
using FavouriteDishBot.Util.Button;
using FavouriteDishBot.Util.String;

namespace FavouriteDishBot.Service;

public class ServiceManager
{
    private Dictionary<string, Func<string, TransmittedData, BotMessage>>
        _methods;

    public ServiceManager()
    {
        FdbDbContext db = new FdbDbContext();

        IDishesListsRepository dishesListsRepository = new DishesListsRepository(db);
        IDishesRepository dishesRepository = new DishesRepository(db);

        StartMenuService startMenuService = new StartMenuService();
        MainMenuService mainMenuService = new MainMenuService(dishesListsRepository);
        ListMenuService listMenuService = new ListMenuService(dishesRepository, dishesListsRepository);
        DishMenuService dishMenuService = new DishMenuService(dishesRepository, dishesListsRepository);
        RandomDishMenuService randomDishMenuService = new RandomDishMenuService(dishesRepository, dishesListsRepository);

        _methods =
            new Dictionary<string, Func<string, TransmittedData, BotMessage>>();

        #region StartMenu
        _methods[States.StartMenu.CommandStart] = startMenuService.ProcessCommandStart;
        #endregion

        #region MainMenu
        _methods[States.MainMenu.ClickOnInlineButtonInMenuMain] = mainMenuService.ProcessClickOnInlineButton;
        #endregion

        #region ListMenu
        _methods[States.ListMenu.InputListName] = listMenuService.ProcessInputListName;
        _methods[States.ListMenu.ClickOnReplyButtonUserLists] = listMenuService.ProcessClickOnReplyButtonUserLists;
        _methods[States.ListMenu.ClickActionButtonWithList] = listMenuService.ProcessClickActionButtonWithList;
        _methods[States.ListMenu.NewListName] = listMenuService.ProcessNewListName;
        _methods[States.ListMenu.ListInputDeletingConfirmation] = listMenuService.ProcessClickOnDeleteListButton;
        #endregion

        #region DishMenu  
        _methods[States.DishMenu.InputTitle] = dishMenuService.ProcessInputTitle;
        _methods[States.DishMenu.InputIngredients] = dishMenuService.ProcessInputIngredients;
        _methods[States.DishMenu.InputRecipe] = dishMenuService.ProcessInputRecipe;
        _methods[States.DishMenu.InputRating] = dishMenuService.ProcessInputRating;
        _methods[States.DishMenu.InputComment] = dishMenuService.ProcessInputComment;
        _methods[States.DishMenu.InputCreatingConfirmation] = dishMenuService.ProcessInputCreatingConfirmation;
        
        _methods[States.DishMenu.ClickOnReplyButtonListDishes] = dishMenuService.ProcessClickOnReplyButtonListDishes;
        _methods[States.DishMenu.ClickInlineButtonInActionWithDishMenu] = dishMenuService.ProcessClickInlineButtonInActionWithDishMenu;
        _methods[States.DishMenu.InputDeletingConfirmation] = dishMenuService.ProcessInputDeletingConfirmation;
        _methods[States.DishMenu.ChooseEditParameter] = dishMenuService.ProcessChooseEditParameter;
        _methods[States.DishMenu.EditingInputTitle] = dishMenuService.ProcessEditingInputTitle;
        _methods[States.DishMenu.EditingInputIngredients] = dishMenuService.ProcessEditingInputIngredients;
        _methods[States.DishMenu.EditingInputRecipe] = dishMenuService.ProcessEditingInputRecipe;
        _methods[States.DishMenu.EditingInputRating] = dishMenuService.ProcessEditingInputRating;
        _methods[States.DishMenu.EditingInputComment] = dishMenuService.ProcessEditingInputComment;
        #endregion

        #region RandomDishMenu
        _methods[States.RandomDishMenu.ClickOnInlineButtonInRandomDishMenu] = randomDishMenuService.ProcessClickOnInlineButtonInRandomDishMenu;
        _methods[States.RandomDishMenu.ClickOnReplyButtonUserLists] = randomDishMenuService.ProcessClickOnReplyButtonUserLists;
        _methods[States.RandomDishMenu.InputRating] = randomDishMenuService.ProcessInputRating;
        _methods[States.RandomDishMenu.InputComment] = randomDishMenuService.ProcessInputComment;
        #endregion
    }

    public BotMessage ProcessBotUpdate(string textData, TransmittedData transmittedData)
    {
        if (textData == SystemStringsStorage.CommandReset)
        {
            transmittedData.State = States.MainMenu.ClickOnInlineButtonInMenuMain;
            transmittedData.DataStorage.Clear();
            return new BotMessage(DialogsStringsStorage.MainMenu, InlineKeyboardMarkupStorage.MainMenuChoose);
        }
        if (transmittedData.State != States.StartMenu.CommandStart && textData==SystemStringsStorage.CommandStart)
        {
            return new BotMessage(DialogsStringsStorage.UnknownCommandInput);
        }
        Func<string, TransmittedData, BotMessage> serviceMethod = _methods[transmittedData.State];
        return serviceMethod.Invoke(textData, transmittedData);
    }
}