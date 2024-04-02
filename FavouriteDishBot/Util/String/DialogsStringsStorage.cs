using FavouriteDishBot.Db.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

namespace FavouriteDishBot.Util.String;

public class DialogsStringsStorage
{
    public const string CommandStartInputErrorInput = "Команда не распознана. Для начала работы с ботом введите /start";
    public const string UnknownCommandInput = "Введена неизвестная команда. Для продолжения работы с ботом введите /reset";

    public const string MainMenu = "Выберите действие";

    public const string ListNameInput = "Введите название списка";

    public const string ListNameInputError = "Название списка не должно превышать 20 символов\n\n";

    public const string ListCreateSuccess = "Список создан успешно!";

    public const string MyLists = "Ваши списки";

    public const string NoLists = "У вас пока нет списков\n\n";

    public const string PressButton = "Нажмите на кнопку";

    public const string ListDeleted = "Список успешно удален";

    public const string ListDeletedConfirmation = "Подтверждаете удаление списка?";

    public static string ChoosedList(DishesList dishesList)
    {
        return "Выбран список\n\n" +
               $"{dishesList.Title}\n\n" +
               "Выберите действие со списком";
    }

    public const string NewListNameInput = "Введите новое название списка";

    public const string ListNameChangeSuccess = "Название списка успешно изменено!";

    public const string DishTitleInput = "Введите название блюда";

    public const string DishTitleInputError = "Название блюда не может превышать 75 символов";

    public const string DishIngredientsInput = "Введите ингредиенты";

    public const string DishRecipeInput = "Введите рецепт";
    
    public const string DishRatingInput = "Введите вашу оценку блюду";

    public const string DishRatingErrorInput = "Оценка не может быть ниже или выше. \n(от 0 до 10)";

    public const string DishCommentInput = "Введите ваш комментарий к данному блюду";

    public const string DishCommentInputError = "Не больше 150 символов";

    public static string CreatedDishParameters(Dish dish)
    {
        return "\nПроверьте правильность введённых данных:\n\n" +
               $"Блюдо: {dish.Title}\n\n" +
               $"Ингредиенты: {dish.Ingredients}\n\n" +
               $"Рецепт {dish.Recipe}\n\n" +
               $"Ваша оценка: {dish.Rating}\n" +
               $"Ваш комментарий: {dish.Comment}\n\n" +
               "Данные введены верно?";
    }
    
    public static string ChoosedDishParameters(Dish dish)
    {
        return $"Выбрано блюдо: {dish.Title}\n\n" +
               $"Игредиенты: {dish.Ingredients}\n\n" +
               $"Рецепт: {dish.Recipe}\n\n" +
               $"Ваша оценка: {dish.Rating}\n" +
               $"Ваш комментарий: {dish.Comment}\n\n" +
               "Выберите действие:";
    }

    public static string RandomDishParameter(Dish dish)
    {
        return $"Случайное блюдо: {dish.Title}\n\n" +
               $"Игредиенты:\n{dish.Ingredients}\n\n" +
               $"Рецепт:\n{dish.Recipe}\n\n" +
               "Выберите действие:";
    }
    
    public static string DishAdded(Dish dish)
    {
        return $"Блюдо {dish.Title} было создано успешно!";
    }

    public static string DishAddedToList(Dish dish,DishesList dishesList)
    {
        return $"Блюдо {dish.Title} было добавлено в {dishesList.Title}";
    }

    public const string DishesInList = "Блюда в этом списке:";
    
    public const string ChooseDishEditParameter = "Выберите параметр который хотите изменить";

    public const string DishAreNull = "В этом списке нет блюд";

    public const string DishDeletingConfirmation = "Подтверждаете удаление блюда?";
}