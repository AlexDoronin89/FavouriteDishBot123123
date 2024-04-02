using FavouriteDishBot.Bot.Router;
using FavouriteDishBot.Bot;
using FavouriteDishBot.Db.DbConnector;
using FavouriteDishBot.Db.Models;
using FavouriteDishBot.Db.Repositories.Implemintations;
using FavouriteDishBot.Db.Repositories.Interfaces;
using FavouriteDishBot.Util.Button;
using FavouriteDishBot.Util.String;
using Telegram.Bot.Types;
using System.Reflection.Metadata;

namespace FavouriteDishBot.Service;

public class DishMenuService
{
    private IDishesRepository _dishesRepository;
    private IDishesListsRepository _dishesListsRepository;

    public DishMenuService(IDishesRepository dishesRepository, IDishesListsRepository dishesListsRepository)
    {
        _dishesRepository = dishesRepository;
        _dishesListsRepository = dishesListsRepository;
    }

    public BotMessage ProcessInputTitle(string textData, TransmittedData transmittedData)
    {
        if (textData.Length > ConstraintStringsStorage.DishTitleMaxLength)
        {
            return new BotMessage(DialogsStringsStorage.DishTitleInputError);
        }

        transmittedData.State = States.DishMenu.InputIngredients;
        transmittedData.DataStorage.AddOrUpdate("dishTitle", textData);

        return new BotMessage(DialogsStringsStorage.DishIngredientsInput);
    }

    public BotMessage ProcessInputIngredients(string textData, TransmittedData transmittedData)
    {
        transmittedData.State = States.DishMenu.InputRecipe;
        transmittedData.DataStorage.AddOrUpdate("dishIngredients", textData );

        return new BotMessage(DialogsStringsStorage.DishRecipeInput);
    }

    public BotMessage ProcessInputRecipe(string textData, TransmittedData transmittedData)
    {
        transmittedData.State = States.DishMenu.InputRating;
        transmittedData.DataStorage.AddOrUpdate("dishRecipe", textData );

        return new BotMessage(DialogsStringsStorage.DishRatingInput);
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

        transmittedData.State = States.DishMenu.InputComment;
        transmittedData.DataStorage.AddOrUpdate("dishRating", inputRating);

        return new BotMessage(DialogsStringsStorage.DishCommentInput);
    }

    public BotMessage ProcessInputComment(string textData, TransmittedData transmittedData)
    {
        if (textData.Length > ConstraintStringsStorage.DishCommentMaxLength)
        {
            return new BotMessage(DialogsStringsStorage.DishCommentInputError);
        }

        transmittedData.State = States.DishMenu.InputCreatingConfirmation;

        Db.Models.Dish dish = new Db.Models.Dish();

        dish.Title = (string)transmittedData.DataStorage.Get("dishTitle");
        dish.Ingredients = (string)transmittedData.DataStorage.Get("dishIngredients");
        dish.Recipe = (string)transmittedData.DataStorage.Get("dishRecipe");
        dish.Rating = (int)transmittedData.DataStorage.Get("dishRating");
        dish.Comment = textData;

        transmittedData.DataStorage.AddOrUpdate("Dish", dish);

        return new BotMessage(DialogsStringsStorage.CreatedDishParameters(dish), InlineKeyboardMarkupStorage.DishConfirmation);
    }

    public BotMessage ProcessInputCreatingConfirmation(string textData, TransmittedData transmittedData)
    {
        if (textData == BotButtonsStorage.DishMenu.Cancel.CallBackData)
        {
            DishesList dishesList = _dishesListsRepository.GetDishesListById((int)transmittedData.DataStorage.Get("listId"));
            transmittedData.State = States.ListMenu.ClickActionButtonWithList;
            return new BotMessage(DialogsStringsStorage.ChoosedList(dishesList), InlineKeyboardMarkupStorage.ListMenuChoose);
        }
        if (textData == BotButtonsStorage.DishMenu.Confirm.CallBackData)
        {
            transmittedData.State = States.ListMenu.ClickActionButtonWithList;

            Db.Models.Dish dish = (Db.Models.Dish)transmittedData.DataStorage.Get("Dish");

            DishesList dishesList = _dishesListsRepository.GetDishesListById((int)transmittedData.DataStorage.Get("listId"));
            _dishesListsRepository.AddDishInDishesList(dishesList, dish);
            transmittedData.DataStorage.Delete("Dish");

            return new BotMessage(DialogsStringsStorage.DishAdded(dishesList.Dishes.Last()), InlineKeyboardMarkupStorage.ListMenuChoose);
        }

        throw new Exception("Неизвестная ошибка в ProcessInputCreatingConfirmation");
    }

    public BotMessage ProcessClickOnReplyButtonListDishes(string textData, TransmittedData transmittedData)
    {
        DishesList dishesList = _dishesListsRepository.GetDishesListById((int)transmittedData.DataStorage.Get("listId"));

        if (textData == BotButtonsStorage.ListMenu.BackToLists.Name)
        {
            transmittedData.State = States.ListMenu.ClickActionButtonWithList;

            return new BotMessage(DialogsStringsStorage.ChoosedList(dishesList), InlineKeyboardMarkupStorage.ListMenuChoose, true);
        }

        Db.Models.Dish dish = _dishesRepository.GetDishByTitle(dishesList, textData);

        if (dish == null)
        {
            return new BotMessage(DialogsStringsStorage.DishesInList, ReplyKeyboardMarkupStorage.CreateKeyboardDishes((List<Db.Models.Dish>)dishesList.Dishes));
        }

        transmittedData.State = States.DishMenu.ClickInlineButtonInActionWithDishMenu;
        transmittedData.DataStorage.AddOrUpdate("dishId", dish.Id);

        return new BotMessage(DialogsStringsStorage.ChoosedDishParameters(dish), InlineKeyboardMarkupStorage.DishMenuChoose, true);
    }

    public BotMessage ProcessClickInlineButtonInActionWithDishMenu(string textData, TransmittedData transmittedData)
    {
        if (textData == BotButtonsStorage.DishMenu.EditDish.CallBackData)
        {
            transmittedData.State = States.DishMenu.ChooseEditParameter;

            return new BotMessage(DialogsStringsStorage.ChooseDishEditParameter, InlineKeyboardMarkupStorage.DishEditing);
        }
        if (textData == BotButtonsStorage.DishMenu.DeleteDish.CallBackData)
        {
            transmittedData.State = States.DishMenu.InputDeletingConfirmation;

            return new BotMessage(DialogsStringsStorage.DishDeletingConfirmation, InlineKeyboardMarkupStorage.DishConfirmation);
        }
        if (textData == BotButtonsStorage.DishMenu.BackToListOfDishes.CallBackData)
        {
            List<Db.Models.Dish> dishes = (List<Db.Models.Dish>)_dishesListsRepository.GetDishesListById((int)transmittedData.DataStorage.Get("listId")).Dishes;

            transmittedData.State = States.DishMenu.ClickOnReplyButtonListDishes;

            return new BotMessage(DialogsStringsStorage.DishesInList, ReplyKeyboardMarkupStorage.CreateKeyboardDishes(dishes));
        }

        throw new Exception("Неизвестная ошибка в ProcessClickInlineButtonInActionWithGameMenu");
    }

    public BotMessage ProcessInputDeletingConfirmation(string textData, TransmittedData transmittedData)
    {
        if (textData == BotButtonsStorage.DishMenu.Confirm.CallBackData)
        {
            _dishesRepository.DeleteDish((int)transmittedData.DataStorage.Get("dishId"));

            List<Db.Models.Dish> dishes = (List<Db.Models.Dish>)_dishesListsRepository.GetDishesListById((int)transmittedData.DataStorage.Get("listId")).Dishes;
            if (dishes.Count == 0)
            {
                DishesList dishesList = _dishesListsRepository.GetDishesListById((int)transmittedData.DataStorage.Get("listId"));

                transmittedData.State = States.ListMenu.ClickActionButtonWithList;

                return new BotMessage(DialogsStringsStorage.ChoosedList(dishesList), InlineKeyboardMarkupStorage.ListMenuChoose);
            }

            transmittedData.State = States.DishMenu.ClickOnReplyButtonListDishes;

            return new BotMessage(DialogsStringsStorage.DishesInList, ReplyKeyboardMarkupStorage.CreateKeyboardDishes(dishes));
        }
        if (textData == BotButtonsStorage.DishMenu.Cancel.CallBackData)
        {
            transmittedData.State = States.DishMenu.ClickInlineButtonInActionWithDishMenu;
            Db.Models.Dish dish = _dishesRepository.GetDishById((int)transmittedData.DataStorage.Get("dishId"));
            return new BotMessage(DialogsStringsStorage.ChooseDishEditParameter, InlineKeyboardMarkupStorage.DishMenuChoose, true);
        }

        throw new Exception("Неизвестная ошибка в ProcessInputDeletingConfirmation");
    }

    public BotMessage ProcessChooseEditParameter(string textData, TransmittedData transmittedData)
    {
        if (textData == BotButtonsStorage.DishMenu.InputTitle.CallBackData)
        {
            transmittedData.State = States.DishMenu.EditingInputTitle;

            return new BotMessage(DialogsStringsStorage.DishTitleInput);
        }
        if (textData == BotButtonsStorage.DishMenu.InputIngredients.CallBackData)
        {
            transmittedData.State = States.DishMenu.EditingInputIngredients;

            return new BotMessage(DialogsStringsStorage.DishIngredientsInput);
        }
        if (textData == BotButtonsStorage.DishMenu.InputRecipe.CallBackData)
        {
            transmittedData.State = States.DishMenu.EditingInputRecipe;

            return new BotMessage(DialogsStringsStorage.DishRecipeInput);
        }
        if (textData == BotButtonsStorage.DishMenu.InputRating.CallBackData)
        {
            transmittedData.State = States.DishMenu.EditingInputRating;

            return new BotMessage(DialogsStringsStorage.DishRatingInput);
        }
        if (textData == BotButtonsStorage.DishMenu.InputComment.CallBackData)
        {
            transmittedData.State = States.DishMenu.EditingInputComment;

            return new BotMessage(DialogsStringsStorage.DishCommentInput);
        }
        if (textData == BotButtonsStorage.DishMenu.Back.CallBackData)
        {
            transmittedData.State = States.DishMenu.ClickInlineButtonInActionWithDishMenu;
            Db.Models.Dish dish = _dishesRepository.GetDishById((int)transmittedData.DataStorage.Get("dishId"));

            return new BotMessage(DialogsStringsStorage.ChoosedDishParameters(dish), InlineKeyboardMarkupStorage.DishMenuChoose);
        }

        throw new Exception("Неизвестная ошибка в ProcessChooseEditParameter");
    }

    public BotMessage ProcessEditingInputTitle(string textData, TransmittedData transmittedData)
    {
        if (textData.Length > ConstraintStringsStorage.DishTitleMaxLength)
        {
            return new BotMessage(DialogsStringsStorage.DishTitleInputError);
        }

        transmittedData.State = States.DishMenu.ChooseEditParameter;

        Db.Models.Dish dish = _dishesRepository.GetDishById((int)transmittedData.DataStorage.Get("dishId"));
        dish.Title = textData;

        _dishesRepository.UpdateDish(dish);

        return new BotMessage(DialogsStringsStorage.ChooseDishEditParameter, InlineKeyboardMarkupStorage.DishEditing);
    }
    public BotMessage ProcessEditingInputIngredients(string textData, TransmittedData transmittedData)
    {
        transmittedData.State = States.DishMenu.ChooseEditParameter;

        Db.Models.Dish dish = _dishesRepository.GetDishById((int)transmittedData.DataStorage.Get("dishId"));
        dish.Ingredients = textData;

        _dishesRepository.UpdateDish(dish);

        return new BotMessage(DialogsStringsStorage.ChooseDishEditParameter, InlineKeyboardMarkupStorage.DishEditing);
    }
    public BotMessage ProcessEditingInputRecipe(string textData, TransmittedData transmittedData)
    {
        transmittedData.State = States.DishMenu.ChooseEditParameter;

        Db.Models.Dish dish = _dishesRepository.GetDishById((int)transmittedData.DataStorage.Get("dishId"));
        dish.Recipe = textData;

        _dishesRepository.UpdateDish(dish);

        return new BotMessage(DialogsStringsStorage.ChooseDishEditParameter, InlineKeyboardMarkupStorage.DishEditing);
    }
    public BotMessage ProcessEditingInputRating(string textData, TransmittedData transmittedData)
    {
        if (int.TryParse(textData, out int inputRating) == false)
        {
            return new BotMessage(DialogsStringsStorage.DishRatingErrorInput);
        }
        if (inputRating < ConstraintStringsStorage.DishMinRating || inputRating > ConstraintStringsStorage.DishMaxRating)
        {
            return new BotMessage(DialogsStringsStorage.DishRatingErrorInput);
        }

        transmittedData.State = States.DishMenu.ChooseEditParameter;

        Db.Models.Dish dish = _dishesRepository.GetDishById((int)transmittedData.DataStorage.Get("dishId"));
        dish.Rating = inputRating;

        _dishesRepository.UpdateDish(dish);
        return new BotMessage(DialogsStringsStorage.ChooseDishEditParameter, InlineKeyboardMarkupStorage.DishEditing);
    }
    public BotMessage ProcessEditingInputComment(string textData, TransmittedData transmittedData)
    {
        if (textData.Length > ConstraintStringsStorage.DishCommentMaxLength)
        {
            return new BotMessage(DialogsStringsStorage.DishCommentInputError);
        }

        transmittedData.State = States.DishMenu.ChooseEditParameter;

        Db.Models.Dish dish = _dishesRepository.GetDishById((int)transmittedData.DataStorage.Get("dishId"));
        dish.Comment = textData;

        _dishesRepository.UpdateDish(dish);
        return new BotMessage(DialogsStringsStorage.ChooseDishEditParameter, InlineKeyboardMarkupStorage.DishEditing);
    }
}