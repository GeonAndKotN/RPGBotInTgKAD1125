using System;

namespace MyBot.Game
{
    public class Monster
    {
        //Выдаём монстрам всё-всё
        public string Name { get; set; }
        public int MaxHealth { get; set; }
        public int Damage { get; set; }
        public string ImageUrl { get; set; }

        public int Reward { get; set; }

        private int health;
        public int CurrentHealth => health;
        // Проверка монстра, если у него хп упадёт ниже нуля, ну или как тут мы видим, оно будет равно 0, его состояние станет IsDead
        public bool IsDead => health <= 0;

        public Monster()
        {
            ResetHealth();
        }

        public void ResetHealth()
        {
            health = MaxHealth;
        }

        public void TakeDamage(int value)
        {
            health -= value;
        }
    }

    public static class Monsters
    {
        public static Monster GetRandom()
        {
            var index = new Random().Next(13);
            switch (index)
            {
                case 1:
                    return Wiwern;
                case 2:
                    return Ghoul;
                case 3:
                    return Dino;
                case 4: 
                    return WrKing; 
                case 5: 
                    return Undying; 
                case 6: 
                    return Slark; 
                case 7: 
                    return MonkeyKing; 
                case 8: 
                    return TreantPr;
                case 9: 
                    return Sven;
                case 10:
                    return Timber;
                case 11:
                    return Muerta;
                case 12:
                    return Io;
                default:
                    return Pudge;
            }
        }
        // хуйня какая-то почему то виверна впринципе падает
        public static Monster Pudge => new Monster() { Name = "ПУДГЕ", MaxHealth = 20, Damage = 8, Reward = 3, ImageUrl = "https://dota2.ru/img/heroes/pudge/pudge.png" };
        public static Monster Ghoul => new Monster() { Name = "ГУЛЯШ", MaxHealth = 13, Damage = 3, Reward = 1, ImageUrl = "https://dota2.ru/img/heroes/lifestealer/lifestealer.png" };
        public static Monster Wiwern => new Monster() { Name = "АЛКОГОЛЬНОЕ ИСПАРЕНИЕ(ещё та тварь)", MaxHealth = 5, Damage = 4, Reward = 1, ImageUrl = "https://dota2.ru/img/heroes/winter_wyvern/winter_wyvern.png" };
        public static Monster Dino => new Monster() { Name = "ГИГАНТСКИЙ ЯЩЕР", MaxHealth = 16, Damage = 6, Reward = 2, ImageUrl = "https://dota2.ru/img/heroes/primal_beast/primal_beast.png" };
        public static Monster WrKing => new Monster() { Name = "НЕДОМЁРТВЫЙ ДЕД", MaxHealth = 11, Damage = 5, Reward = 2, ImageUrl = "https://dota2.ru/img/heroes/wraith_king/wraith_king.png" };
        public static Monster Undying => new Monster() { Name = "ЗОМБЕЕЕЕ", MaxHealth = 11, Damage = 4, Reward = 3, ImageUrl = "https://dota2.ru/img/heroes/undying/undying.png" };
        public static Monster Slark => new Monster() { Name = "ОКУНЬ(ЖИВОЙ)", MaxHealth = 12, Damage = 5, Reward = 5, ImageUrl = "https://dota2.ru/img/heroes/slark/slark.png" }; 
        public static Monster MonkeyKing => new Monster() { Name = "СТУДЕНТ, НЕСДАВШИЙ КУРСАЧ", MaxHealth = 11, Damage = 3, Reward = 2, ImageUrl = "https://dota2.ru/img/heroes/monkey_king/monkey_king.png" };
        public static Monster TreantPr => new Monster() { Name = "(НЕ)МУДРЫЙ ДУБ", MaxHealth = 40, Damage = 1, Reward = 4, ImageUrl = "https://dota2.ru/img/heroes/treant_protector/treant_protector.png" }; 
        public static Monster Sven => new Monster() { Name = "БЕРСЕРК", MaxHealth = 25, Damage = 3, Reward = 9, ImageUrl = "https://dota2.ru/img/heroes/sven/sven.png" };
        public static Monster Timber => new Monster() { Name = "БЕШЕННЫЙ ЛЕСОРУБ", MaxHealth = 15, Damage = 9, Reward = 6, ImageUrl = "https://dota2.ru/img/heroes/timbersaw/timbersaw.png" };
        public static Monster Muerta => new Monster() { Name = "МЁРТВАЯ МАТЬ", MaxHealth = 25, Damage = 3, Reward = 4, ImageUrl = "https://dota2.ru/img/heroes/muerta/muerta.png" };
        public static Monster Io => new Monster() { Name = "ЛЕТАЮЩАЯ ОТРЫЖКА", MaxHealth = 15, Damage = 4, Reward = 2, ImageUrl = "https://dota2.ru/img/heroes/io/io.png" };
        
    }
}
