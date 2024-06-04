using Telegram.Bot.Types.ReplyMarkups;

namespace MyBot.Game
{
    public abstract class Location
    {
        public virtual string ImageUrl { get; }
        public virtual string Description { get; }
        public virtual List<InlineKeyboardButton> GetButtons()
        {
            return new List<InlineKeyboardButton> { Buttons.Menu };
        }
    }

    public static class Locations
    {
        public static Location Home { get; set; } = new Home();
        public static Location School { get; set; } = new School();
        public static Location Shop { get; set; } = new Shop();
        public static Location Arena { get; set; } = new Arena();
    }

    public enum LocationCommand
    {
        //Магаз
        Buyarmor = 101,
        Buyweapon = 102,
        Buypotion = 103,

        //ппк
        LearnStr = 201,
        LearnAgi = 202,
        LearnIntel = 203,
        LearnPhy = 204,

        //Боёвка типо мортал комбат, но пошаговое рпг мочилово
        Attack = 301,
        Defence = 302,
        Usepotion = 303,
    }

    public static class LocationCommandExtensions
    {
        public static bool IsShopCommand(this LocationCommand command)
        {
            switch (command)
            {
                case LocationCommand.Buyarmor:
                case LocationCommand.Buyweapon:
                case LocationCommand.Buypotion:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsSchoolCommand(this LocationCommand command)
        {
            switch (command)
            {
                case LocationCommand.LearnStr:
                case LocationCommand.LearnAgi:
                case LocationCommand.LearnIntel:
                case LocationCommand.LearnPhy:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsArenaCommand(this LocationCommand command)
        {
            switch (command)
            {
                case LocationCommand.Attack:
                case LocationCommand.Defence:
                case LocationCommand.Usepotion:
                    return true;
                default:
                    return false;
            }
        }
    }
}
