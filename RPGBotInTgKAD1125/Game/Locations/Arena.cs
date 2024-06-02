using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyBot.Game
{
    public class Arena : Location
    {
        public override string ImageUrl => "https://raw.githubusercontent.com/GeonAndKotN/BotInTg/master/arena.jpg";
        public override string Description => "Добро пожаловать на ♂арену♂.\nТут много кого уже победили";
        public override List<InlineKeyboardButton> GetButtons()
        {
            var result = new List<InlineKeyboardButton> {
                Buttons.Attack,
                Buttons.Defence,
                Buttons.UsePotion,
                Buttons.Run
            };
            return result;
        }
    }
}
