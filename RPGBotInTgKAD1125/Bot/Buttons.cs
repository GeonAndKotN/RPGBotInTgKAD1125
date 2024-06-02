using Telegram.Bot.Types.ReplyMarkups;

namespace MyBot
{
    public static class Buttons
    {
        //Игрок
        public static InlineKeyboardButton Menu => InlineKeyboardButton.WithCallbackData("Меню", "/Menu");
        public static InlineKeyboardButton Play => InlineKeyboardButton.WithCallbackData("К игре", "/Play");
        public static InlineKeyboardButton CreateChar => InlineKeyboardButton.WithCallbackData("Создать", "/CreateChar");
        public static InlineKeyboardButton DeleteChar => InlineKeyboardButton.WithCallbackData("Удалить", "/DeleteChar");
        public static InlineKeyboardButton CharInfo => InlineKeyboardButton.WithCallbackData("Выбрать", "/CharInfo");

        //Локации
        public static InlineKeyboardButton GoHome => InlineKeyboardButton.WithCallbackData("Домой", "/GoHome");
        public static InlineKeyboardButton GoSchool => InlineKeyboardButton.WithCallbackData("🤓Школа🤓", "/GoSchool");
        public static InlineKeyboardButton GoShop => InlineKeyboardButton.WithCallbackData("📈Магазин📉", "/GoShop");
        public static InlineKeyboardButton GoArena => InlineKeyboardButton.WithCallbackData("💥Арена💥", "/GoArena");

        //Магаз
        public static InlineKeyboardButton BuyArmor => InlineKeyboardButton.WithCallbackData("🛡️Броня🛡️", "/BuyArmor");
        public static InlineKeyboardButton BuyWeapon => InlineKeyboardButton.WithCallbackData("🖋️Оружие✒️", "/BuyWeapon");
        public static InlineKeyboardButton BuyPotion => InlineKeyboardButton.WithCallbackData("🍼Зелье🍷", "/BuyPotion");


        //Школа
        public static InlineKeyboardButton LearnStr => InlineKeyboardButton.WithCallbackData("📕Сила📕", "/LearnStr");
        public static InlineKeyboardButton LearnAgi => InlineKeyboardButton.WithCallbackData("📗Ловкость📗", "/LearnAgi");
        public static InlineKeyboardButton LearnIntel => InlineKeyboardButton.WithCallbackData("📘Интеллект📘", "/LearnIntel");
        public static InlineKeyboardButton LearnPhy => InlineKeyboardButton.WithCallbackData("📙Стойкость📙", "/LearnPhy");


        //Арена
        public static InlineKeyboardButton Attack => InlineKeyboardButton.WithCallbackData("💥Удар💥", "/Attack");
        public static InlineKeyboardButton Defence => InlineKeyboardButton.WithCallbackData("🛡️Блок🛡️", "/Defence");
        public static InlineKeyboardButton UsePotion => InlineKeyboardButton.WithCallbackData("🍼Зелье🍷", "/UsePotion");
        public static InlineKeyboardButton Run => InlineKeyboardButton.WithCallbackData("СВАЛИТЬ", "/GoHome");
    }
}
