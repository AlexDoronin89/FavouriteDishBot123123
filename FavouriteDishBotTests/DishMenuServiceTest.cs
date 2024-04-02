using FavouriteDishBot.Bot.Router;
using FavouriteDishBot.Db.Models;
using FavouriteDishBot.Db.Repositories.Interfaces;
using FavouriteDishBot.Service;
using Moq;
using Xunit;

namespace FavouriteDishBotTests
{
    public class DishMenuServiceTest
    {
        [Fact]
        public void ProcessEditingInputIngredients_ReturnUpdatedIngredients()
        {
            Dish dish = new Dish {
                Id = 0,
                Title = "123",
                Ingredients = null,
                Comment = null,
                Rating = 123
            };

            Mock<IDishesRepository> dishesRepository=new Mock<IDishesRepository>();
            dishesRepository.Setup(exp=>exp.GetDishById(0)).Returns(dish);

            DishMenuService dishMenuService = new DishMenuService(dishesRepository.Object,null);

            TransmittedData transmittedData = new TransmittedData(0);
            transmittedData.DataStorage.AddOrUpdate("dishId",0);

            string textData = "абвгд";

            dishMenuService.ProcessEditingInputIngredients(textData, transmittedData);

            Assert.Equal(textData,dish.Ingredients);
        }
    }
}
