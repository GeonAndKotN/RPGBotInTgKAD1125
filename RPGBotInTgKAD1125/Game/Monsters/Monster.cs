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
            var index = new Random().Next(4);
            switch (index)
            {
                case 1:
                    return Wiwern;
                case 2:
                    return Ghoul;
                case 3:
                    return Dino;
                default:
                    return Pudge;
            }
        }
        // хуйня какая-то почему то виверна впринципе падает
        public static Monster Pudge => new Monster() { Name = "ПУДГЕ", MaxHealth = 20, Damage = 8, Reward = 3, ImageUrl = "https://dota2.ru/img/heroes/pudge/pudge.png" };
        public static Monster Ghoul => new Monster() { Name = "ГУЛЯШ", MaxHealth = 13, Damage = 3, Reward = 1, ImageUrl = "https://dota2.ru/img/heroes/lifestealer/lifestealer.png" };
        public static Monster Wiwern => new Monster() { Name = "АЛКОГОЛЬНОЕ ИСПАРЕНИЕ(ещё та тварь)", MaxHealth = 5, Damage = 4, Reward = 1, ImageUrl = "https://dota2.ru/img/heroes/winter_wyvern/winter_wyvern.png" };
        public static Monster Dino => new Monster() { Name = "ГИГАНТСКИЙ ЯЩЕР", MaxHealth = 16, Damage = 6, Reward = 2, ImageUrl = "https://dota2.ru/img/heroes/primal_beast/primal_beast.png" };
    }
}
