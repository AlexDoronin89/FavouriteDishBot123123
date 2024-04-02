using FavouriteDishBot.Db.DbConnector;
using FavouriteDishBot.Db.Models;
using FavouriteDishBot.Db.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace FavouriteDishBot.Db.Repositories.Implemintations;

public class DishesRepository : IDishesRepository
{
    private FdbDbContext _dbContext;

    public DishesRepository(FdbDbContext db)
    {
        _dbContext = db;
    }

    public List<Dish> GetDishesByDishesListId(int dishesListId)
    {
        DishesList dishesList = _dbContext.DishesLists.Where(x => x.Id == dishesListId).Include(x => x.Dishes).FirstOrDefault();
        return dishesList.Dishes as List<Dish>;
    }
    public Dish GetDishById(int id)
    {
        return _dbContext.Dishes.Where(x=>x.Id == id).FirstOrDefault();
    }

    public Dish GetDishByTitle(DishesList dishesList, string title)
    {
        foreach (Dish dish in dishesList.Dishes)
        {
            if (dish.Title == title)
            {
                return dish;
            }
        }

        return null;
    }

    public void AddDish(int listId, string title,string ingredients,string recipe, int rating, string comment)
    {
        DishesList dishesList = _dbContext.DishesLists.Where(x => x.Id == listId).FirstOrDefault();

        dishesList.Dishes.Add(new Dish()
        {
            Title = title,
            Ingredients = ingredients,
            Recipe = recipe,
            Rating = rating,
            Comment = comment
        });

        _dbContext.DishesLists.Update(dishesList);
        _dbContext.SaveChanges();
    }

    public void UpdateDish(Dish dish)
    {
        _dbContext.Dishes.Update(dish);
        _dbContext.SaveChanges();
    }

    public void DeleteDish(int dishId)
    {
        Dish dish = _dbContext.Dishes.Where(x => x.Id == dishId).FirstOrDefault();
        _dbContext.Dishes.Remove(dish);
        _dbContext.SaveChanges();
    }
}