namespace FavouriteDishBot.Util.Button;

public static class BotButtonsStorage
{
    public static MainMenu MainMenu { get; } = new();
    public static ListMenu ListMenu { get; } = new();
    public static DishMenu DishMenu { get; } = new();
    public static RandomDish RandomDish { get; } = new();
    public static SubmitData SubmitData { get; } = new();
    public static System System { get; } = new();
}

public class MainMenu
{
    public BotButton CreateDishList { get; } = new("Создать список", "CreateDishList");
    public BotButton Lists { get; } = new("Просмотреть свои списки", "Lists");
    public BotButton RandomDish { get; } = new("Получить случайное блюдо", "RandomDish");
}

public class ListMenu
{
    public BotButton AddDish { get; } = new("Добавить блюдо", "AddDish");
    public BotButton CheckDishes { get; } = new("Посмотреть блюда", "CheckDishes");
    public BotButton RenameList { get; } = new("Переименовать список", "RenameList");
    public BotButton DeleteList { get; } = new("Удалить список", "DeleteList");
    public BotButton BackToLists { get; } = new("Назад", "BackToLists");

    public BotButton List { get; } = new("Список", "ListBtn");
    public BotButton Dish { get; } = new("Брауни", "Game");
    public BotButton BackToList { get; } = new("Назад", "BackToList");
}

public class DishMenu
{
    public BotButton EditDish { get; } = new("Изменить", "EditDish");
    public BotButton DeleteDish { get; } = new("Удалить", "DeleteDish");
    public BotButton BackToListOfDishes { get; } = new("Назад", "BackToListOfDishes");
    public BotButton Confirm { get; } = new("Подтверждаю", "Confirm");
    public BotButton Cancel { get; } = new("Отменить", "Cancel");
    public BotButton InputTitle { get; } = new("Название", "InputTitle");
    public BotButton InputIngredients { get; } = new("Ингредиенты", "InputIngredients");
    public BotButton InputRecipe { get; } = new("Рецепт", "InputRecipe");
    public BotButton InputRating { get; } = new("Оценка", "InputRating");
    public BotButton InputComment { get; } = new("Комментарий", "InputComment");
    public BotButton Back { get; } = new("Назад", "Back");
}

public class RandomDish
{
    public BotButton AddToDishList { get; } = new("Добавить в список", "AddToDishList");
}

public class SubmitData
{
    public BotButton CorrectData { get; } = new("Да", "CorrectData");
    public BotButton NotCorrectData { get; } = new("Нет", "NotCorrectData");
}

public class System
{
    public BotButton ButtonBack { get; } = new("Обратно в главное меню", "ButtonBack");
}