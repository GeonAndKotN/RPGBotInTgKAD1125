using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyBot.Game
{
    public class Shop : Location
    {
        public override string ImageUrl => "https://raw.githubusercontent.com/GeonAndKotN/BotInTg/master/shop.jpg";
        public override string Description => "Добро пожаловать в наш любимый ☭Магазин☭\nЧто хотим улучшить?" +
            "\n\nКаждая броня добавляет по 5 максимального здоровья" +
            "\nКаждое оружие добавляет по 2 к значению урона" +
            "\nКаждое зелье будет лучше хилить предыдущего" +
            "\n\nКаждая новая броня будет стоить на 3 дороже предыдущей, а оружие 4." +
            "\nСтоимость зелья: '20','50','70'";
        public override List<InlineKeyboardButton> GetButtons()
        {
            var result = new List<InlineKeyboardButton> {
                Buttons.BuyArmor,
                Buttons.BuyWeapon,
                Buttons.BuyPotion,
                Buttons.GoHome,
            };
            return result;
        }
    }
}
