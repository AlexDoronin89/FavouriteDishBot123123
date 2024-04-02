using FavouriteDishBot.Db.Models;

namespace FavouriteDishBot.Bot.Router;

public class States
{
    public static StartMenu StartMenu { get; } = new();
    public static MainMenu MainMenu { get; } = new();
    public static ListMenu ListMenu { get; } = new();
    public static DishMenu DishMenu { get; } = new();
    public static RandomDishMenu RandomDishMenu { get; } = new();
}

public class StartMenu
{
    public string CommandStart { get; } = "CommandStart";
}

public class MainMenu
{
    public string ClickOnInlineButtonInMenuMain { get; } = "ClickOnInlineButtonInMenuMain";
}

public class ListMenu
{
    public string InputListName { get; } = "InputListName";
    public string ClickOnReplyButtonUserLists { get; } = "ClickOnInlineButtonUserLists";
    public string ClickActionButtonWithList { get; } = "ClickActionButtonWithList";
    public string NewListName { get; } = "NewListName";
    public string ListInputDeletingConfirmation { get; } = "ListInputDeletingConfirmation";
}

public class DishMenu
{
    public string ClickOnReplyButtonListDishes { get; } = "ClickButtonDisheInListDishesMenu";
    public string ClickInlineButtonInActionWithDishMenu { get; } = "ClickInlineButtonInActionWithDishMenu";
    public string ChooseEditParameter { get; } = "ChooseEditParameter";
    public string InputTitle { get; } = "InputTitle";
    public string InputIngredients { get; } = "InputIngredients";
    public string InputRecipe { get; } = "InputRecipe";
    public string InputRating { get; } = "InputRating";
    public string InputComment { get; } = "InputComment";
    public string EditingInputTitle { get; } = "EditingInputTitle";
    public string EditingInputIngredients { get; } = "EditingInputDishIngredients";
    public string EditingInputRecipe { get; } = "EditingInputDishRecipe";
    public string EditingInputRating { get; } = "EditingInputRating";
    public string EditingInputComment { get; } = "EditingInputComment";
    public string InputCreatingConfirmation { get; } = "InputCreatingConfirmation";
    public string InputDeletingConfirmation { get; } = "InputDeletingConfirmation";
}

public class RandomDishMenu
{
    public string ClickOnInlineButtonInRandomDishMenu { get; } = "ClickOnInlineButtonInRandomDishMenu";
    public string ClickOnReplyButtonUserLists { get; } = "ClickOnReplyButtonUserLists";
    public string InputRating { get; } = "InputRating";
    public string InputComment { get; } = "InputComment";
}