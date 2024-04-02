using System;
using System.Threading.Tasks;
using FavouriteDishBot.Bot;

BotInitializer bot = new BotInitializer();
bot.Start();

TaskCompletionSource tcs = new TaskCompletionSource();

AppDomain.CurrentDomain.ProcessExit += (_, _) =>
{
    bot.Stop();
    Console.WriteLine("Bot stopped");
    tcs.SetResult();
};

Console.WriteLine("Press CTRL+C to stop");

await tcs.Task;

Console.WriteLine("program finished");