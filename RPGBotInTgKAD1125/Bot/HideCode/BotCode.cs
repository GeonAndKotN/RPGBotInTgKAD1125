﻿using MyBot.Bot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGBotInTgKAD1125.Bot.HideCode
{
    public static class BotCode
    {
        //Простая фигнюшка, при помощи которой можно было спрятать код.
        public const string botCode = "7106838499:AAEpYHq5JEYgfMrnUmE1R0NtrkpNsz-v9sU";

        //7106838499:AAEpYHq5JEYgfMrnUmE1R0NtrkpNsz-v9sU ПРОВЕРКА

        //7140884239:AAFMcWNsUnDo7rFrDQGRlpYovz1C0KewLIQ realbot
        public static int GenerateCharId() => new Random().Next(int.MinValue,int.MaxValue);
    }
}
