using FavouriteDishBot.Bot;
using FavouriteDishBot.Bot.Router;
using FavouriteDishBot.Db.Models;
using FavouriteDishBot.Db.Repositories.Interfaces;
using FavouriteDishBot.Service;
using FavouriteDishBot.Util.String;
using Moq;
using Xunit;

namespace FavouriteDishBotTests
{
    public class ListMenuServiceTest
    {
        [Fact]
        public void ProcessClickOnReplyButtonUserLists_ReturnUserLists()
        {

            DishesList dishesList = new DishesList{
                ChatId = 0,
                Title = "123",
            };

            Mock<IDishesListsRepository> dishesListRepository = new Mock<IDishesListsRepository>();
                dishesListRepository.Setup(exp => exp.GetDishesListByTitle(dishesList.Title,0)).Returns(dishesList);


            ListMenuService listMenuService = new ListMenuService(null, dishesListRepository.Object);

            TransmittedData transmittedData = new TransmittedData(0);
            transmittedData.DataStorage.AddOrUpdate("listId",0);
            
            string textData = "123";

            string expectedText = DialogsStringsStorage.ChoosedList(dishesList);

            BotMessage botMessage = listMenuService.ProcessClickOnReplyButtonUserLists(textData, transmittedData);
            string actualText = botMessage.Text;

            Assert.Equal(expectedText, actualText);
        }
    }
}
