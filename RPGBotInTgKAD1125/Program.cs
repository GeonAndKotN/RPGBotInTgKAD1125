using MyBot.Bot;
using System;

namespace MyBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Бот был запущен!");
            new BotPresenter();
            Console.ReadLine();
        }
    }
}
