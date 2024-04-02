using FavouriteDishBot.Bot.Router;
using FavouriteDishBot.Db.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace FavouriteDishBot.Util.Button;

public class ReplyKeyboardMarkupStorage
{
    public static ReplyKeyboardMarkup CreateKeyboardDishesLists(List<DishesList> dishesLists)
    {
        var rows = new List<KeyboardButton[]>();
    
        for (var i = 0; i < dishesLists.Count; i++)
        {
            rows.Add(new[] { new KeyboardButton(dishesLists[i].Title) });
        }
    
        rows.Add(new[] { new KeyboardButton("Назад") });
    
        return new ReplyKeyboardMarkup(rows.ToArray()) { ResizeKeyboard = true };
    }
    public static ReplyKeyboardMarkup CreateKeyboardDishes(List<Dish> dishes)
    {
        var rows = new List<KeyboardButton[]>();
    
        for (var i = 0; i < dishes.Count; i++)
        {
            rows.Add(new[] { new KeyboardButton(dishes[i].Title) });
        }
        rows.Add(new[] { new KeyboardButton("Назад") });
    
        return new ReplyKeyboardMarkup(rows.ToArray()) { ResizeKeyboard = true };
    }
}