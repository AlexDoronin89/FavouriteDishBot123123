using FavouriteDishBot.Bot;
using FavouriteDishBot.Bot.Router;
using FavouriteDishBot.Db.Models;
using FavouriteDishBot.Db.Repositories.Interfaces;
using FavouriteDishBot.Service;
using FavouriteDishBot.Util.Button;
using FavouriteDishBot.Util.String;
using Moq;
using System.Collections.Generic;
using Xunit;


namespace FavouriteDishBotTests
{
    public class RandomDishServiceTest
    {
        [Fact]
        public void ProcessClickOnInlineButtonInRandomDishMenu_ReturnBotMessage()
        {
            List<DishesList> dishesLists = new List<DishesList>
            {
                new DishesList
                {
                    ChatId = 0,
                    Title = "123"
                }
            };

            Mock<IDishesListsRepository> dishesListsRepository=new Mock<IDishesListsRepository>();
            dishesListsRepository.Setup(exp=>exp.GetDishesListsByChatId(0)).Returns(dishesLists);

            RandomDishMenuService randomDishMenuService = new RandomDishMenuService(null,dishesListsRepository.Object);

            TransmittedData transmittedData = new TransmittedData(0);

            string textData = BotButtonsStorage.RandomDish.AddToDishList.CallBackData;

            string expectedMessage = new BotMessage(DialogsStringsStorage.MyLists, ReplyKeyboardMarkupStorage.CreateKeyboardDishesLists(dishesLists)).Text;

            string actualMessage = randomDishMenuService.ProcessClickOnInlineButtonInRandomDishMenu(textData,transmittedData).Text;
        
            Assert.Equal(expectedMessage,actualMessage);
        }
    }
}