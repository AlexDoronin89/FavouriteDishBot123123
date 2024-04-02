using FavouriteDishBot.Bot.Router;
using FavouriteDishBot.Db.Models;
using FavouriteDishBot.Db.Repositories.Interfaces;
using FavouriteDishBot.Service;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace FavouriteDishBotTests
{
    public class MainMenuServiceTest
    {
        [Fact]
        public void ProcessClickOnInlineButton_ReturnException()
        {
            List<DishesList> dishesList = new List<DishesList>
            {
                new DishesList
                {
                    ChatId = 0,
                    Title = "123"
                }
            };

            Mock<IDishesListsRepository> dishesListRepository = new Mock<IDishesListsRepository>();
            dishesListRepository.Setup(exp => exp.GetDishesListsByChatId(0)).Returns(dishesList);

            MainMenuService mainMenuService = new MainMenuService(dishesListRepository.Object);

            TransmittedData transmittedData = new TransmittedData(0);
            string textData = "wrong";

            Assert.Throws<Exception>(()=>mainMenuService.ProcessClickOnInlineButton(textData,transmittedData));
        }
    }
}
