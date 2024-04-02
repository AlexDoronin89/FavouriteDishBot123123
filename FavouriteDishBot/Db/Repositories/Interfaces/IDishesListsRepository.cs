using System.Collections.Generic;
using FavouriteDishBot.Db.Models;

namespace FavouriteDishBot.Db.Repositories.Interfaces;

public interface IDishesListsRepository
{
    List<DishesList> GetDishesListsByChatId(long chatId);
    DishesList GetDishesListByTitle(string title, long chatId);
    DishesList GetDishesListById(int id);
    void AddDishInDishesList(DishesList dishesList, Dish dish);
    void AddDishesList(long chatId, string title);
    void UpdateDishesListTitle(int dishesListId, string title);
    void DeleteDishesList(int dishesListId);
}