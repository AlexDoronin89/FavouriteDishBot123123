using System.Collections.Generic;
using System.Linq;
using FavouriteDishBot.Db.DbConnector;
using FavouriteDishBot.Db.Models;
using FavouriteDishBot.Db.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FavouriteDishBot.Db.Repositories.Implemintations;

public class DishesListsRepository : IDishesListsRepository
{
    private FdbDbContext _dbContext;

    public DishesListsRepository(FdbDbContext db)
    {
        _dbContext = db;
    }



    public List<DishesList> GetDishesListsByChatId(long chatId)
    {
        return _dbContext.DishesLists.Where(x => x.ChatId == chatId).ToList();
    }

    public DishesList GetDishesListByTitle(string title, long chatId)
    {
        return _dbContext.DishesLists.Where(x => x.Title == title && x.ChatId == chatId).FirstOrDefault();
    }

    public DishesList GetDishesListById(int id)
    {
        return _dbContext.DishesLists.Where(x => x.Id == id).Include(x => x.Dishes).FirstOrDefault();
    }

    public void AddDishInDishesList(DishesList dishesList, Dish dish)
    {
        dishesList.Dishes.Add(dish);
        _dbContext.DishesLists.Update(dishesList);
        _dbContext.SaveChanges();
    }

    public void AddDishesList(long chatId, string title)
    {
        _dbContext.DishesLists.Add(new DishesList()
        {
            ChatId = chatId,
            Title = title,
        });
        _dbContext.SaveChanges();
    }

    public void UpdateDishesListTitle(int dishesListId, string title)
    {
        DishesList dishesList = _dbContext.DishesLists.Where(x => x.Id == dishesListId).FirstOrDefault();
        dishesList.Title = title;
        _dbContext.DishesLists.Update(dishesList);
        _dbContext.SaveChanges();
    }

    public void DeleteDishesList(int dishesListId)
    {
        DishesList dishesList = _dbContext.DishesLists.Where(x => x.Id == dishesListId).Include(x => x.Dishes).FirstOrDefault();
        List<Dish> dishes = (List<Dish>)dishesList.Dishes;
        _dbContext.DishesLists.Remove(dishesList);
        _dbContext.SaveChanges();

        foreach (var dish in dishes)
        {
            _dbContext.Dishes.Remove(dish);
            _dbContext.SaveChanges();
        }
    }
}