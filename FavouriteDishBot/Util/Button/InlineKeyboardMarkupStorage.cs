using Telegram.Bot.Types.ReplyMarkups;

namespace FavouriteDishBot.Util.Button;

public class InlineKeyboardMarkupStorage
{
    public static InlineKeyboardMarkup MainMenuChoose = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.MainMenu.CreateDishList.Name,
                BotButtonsStorage.MainMenu.CreateDishList.CallBackData),
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.MainMenu.Lists.Name,
                BotButtonsStorage.MainMenu.Lists.CallBackData)
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.MainMenu.RandomDish.Name,
                BotButtonsStorage.MainMenu.RandomDish.CallBackData)
        }
    });

    public static InlineKeyboardMarkup ChooseList = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ListMenu.List.Name,
                BotButtonsStorage.ListMenu.List.CallBackData),
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.System.ButtonBack.Name,
                BotButtonsStorage.System.ButtonBack.CallBackData)
        }
    });

    public static InlineKeyboardMarkup ListMenuChoose = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ListMenu.AddDish.Name,
                BotButtonsStorage.ListMenu.AddDish.CallBackData)
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ListMenu.CheckDishes.Name,
                BotButtonsStorage.ListMenu.CheckDishes.CallBackData)
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ListMenu.RenameList.Name,
                BotButtonsStorage.ListMenu.RenameList.CallBackData)
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ListMenu.DeleteList.Name,
                BotButtonsStorage.ListMenu.DeleteList.CallBackData)
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ListMenu.BackToLists.Name,
                BotButtonsStorage.ListMenu.BackToLists.CallBackData)
        }

    });

    public static InlineKeyboardMarkup ListOfDishesChoose = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ListMenu.Dish.Name,
                BotButtonsStorage.ListMenu.Dish.CallBackData),
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ListMenu.BackToList.Name,
                BotButtonsStorage.ListMenu.BackToList.CallBackData)
        }
    });

    public static InlineKeyboardMarkup DishMenuChoose = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.DishMenu.EditDish.Name,
                BotButtonsStorage.DishMenu.EditDish.CallBackData),
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.DishMenu.DeleteDish.Name,
                BotButtonsStorage.DishMenu.DeleteDish.CallBackData),
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.DishMenu.BackToListOfDishes.Name,
                BotButtonsStorage.DishMenu.BackToListOfDishes.CallBackData)
        }
    });

    public static InlineKeyboardMarkup DishConfirmation = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.DishMenu.Confirm.Name,
                BotButtonsStorage.DishMenu.Confirm.CallBackData),
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.DishMenu.Cancel.Name,
                BotButtonsStorage.DishMenu.Cancel.CallBackData),
        }
    });

    public static InlineKeyboardMarkup DishEditing = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.DishMenu.InputTitle.Name,
                BotButtonsStorage.DishMenu.InputTitle.CallBackData),
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.DishMenu.InputIngredients.Name,
                BotButtonsStorage.DishMenu.InputIngredients.CallBackData)
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.DishMenu.InputRecipe.Name,
                BotButtonsStorage.DishMenu.InputRecipe.CallBackData),
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.DishMenu.InputRating.Name,
                BotButtonsStorage.DishMenu.InputRating.CallBackData),
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.DishMenu.InputComment.Name,
                BotButtonsStorage.DishMenu.InputComment.CallBackData),
              InlineKeyboardButton.WithCallbackData(BotButtonsStorage.DishMenu.Back.Name,
                BotButtonsStorage.DishMenu.Back.CallBackData)
        }
    });

    public static InlineKeyboardMarkup RandomDish = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.RandomDish.AddToDishList.Name,
                BotButtonsStorage.RandomDish.AddToDishList.CallBackData),
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.System.ButtonBack.Name,
                BotButtonsStorage.System.ButtonBack.CallBackData)
        }
    });


    public static InlineKeyboardMarkup SubmitDataChoose = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.SubmitData.CorrectData.Name,
                BotButtonsStorage.SubmitData.CorrectData.CallBackData),
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.SubmitData.NotCorrectData.Name,
                BotButtonsStorage.SubmitData.NotCorrectData.CallBackData)
        }
    });

    public static InlineKeyboardMarkup ChooseListToAddDish = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ListMenu.List.Name,
                BotButtonsStorage.ListMenu.List.CallBackData),
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.SubmitData.NotCorrectData.Name,
                BotButtonsStorage.SubmitData.NotCorrectData.CallBackData)
        }
    });
}