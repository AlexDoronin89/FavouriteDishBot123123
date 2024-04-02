using FavouriteDishBot.Bot;
using FavouriteDishBot.Bot.Router;
using FavouriteDishBot.Service;
using FavouriteDishBot.Util.String;
using Xunit;

namespace FavouriteDishBotTests
{
    public class StartMenuServiceTest
    {
        [Fact]
        public void ProcessCommandStart_ReturnStatesMainMenuClickOnInlineButtonInMenuMain()
        {
            StartMenuService startMenuService = new();

            TransmittedData transmittedData = new(0);
            string textData = SystemStringsStorage.CommandStart;

            BotMessage botMessage = startMenuService.ProcessCommandStart(textData, transmittedData);

            string expectedState = States.MainMenu.ClickOnInlineButtonInMenuMain;
            string actualState = transmittedData.State;

            Assert.Equal(expectedState, actualState);
        }
    }
}