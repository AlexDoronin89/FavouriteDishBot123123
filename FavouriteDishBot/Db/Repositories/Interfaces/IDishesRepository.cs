using FavouriteDishBot.Db.Models;

namespace FavouriteDishBot.Db.Repositories.Interfaces;

public interface IDishesRepository
{
    List<Dish> GetDishesByDishesListId(int dishesListId);
    Dish GetDishById(int id);
    Dish GetDishByTitle(DishesList dishesList, string title);
    void AddDish(int listId, string title,string ingredients,string recipe, int rating, string comment);
    void UpdateDish(Dish dish);
    void DeleteDish(int dishId);
}