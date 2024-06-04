﻿using MyBot.DataBase;
using MyBot.Game;
using RPGBotInTgKAD1125.Bot.HideCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyBot.Bot
{
    public class BotModel
    {
        public event EventHandler<ResponseEventArgs> OnReadyToResponse;

        private readonly SQLManager SQLManager = new SQLManager();
        private readonly Dictionary<long, Player> LoadedPlayers = new Dictionary<long, Player>();

        public void Update(object sender, RequestEventArgs args)
        {
            var userId = args.ClientInfo.UserId;
            if (!LoadedPlayers.TryGetValue(userId, out var player))
            {//ищем игрока и суём его в словарь, если он еще не там
                var data = SQLManager.LoadPlayerData(userId);
                player = new Player(data);
                LoadedPlayers.Add(userId, player);
            }
            if (player.ActiveCharacter == null)
            {
                var charsData = player.GetData().characters;
                if (charsData.Count > 0)
                {
                    player.SetActiveCharacter(new Character(charsData.Last()));
                }
                else if (args.Command != GameCommand.CreateChar && (player.CurrentState != PlayerState.CreatingNewChar || string.IsNullOrEmpty(args.Text)))
                {
                    Response(args.ClientInfo, "Чтобы играть, нужен перс, введи имя и всё будет хорошо");
                    player.TakeAction(PlayerAction.CreateChar);
                    return;
                }
            }

            if (args.Command.HasValue)
            {
                if (args.Command.Value.IsPlayerAction(out var playerAction))
                {                  //Если команда - это действие игрока
                    HandlePlayerAction(player, playerAction.Value, args);
                }
                else if (args.Command.Value.IsCharacterAction(out var characterAction))
                {     //если это действие перса
                    HandleCharacterAction(player.ActiveCharacter, characterAction.Value, args);
                }
                else if (args.Command.Value.IsLocationCommand(out var locationCommad))
                {
                    HandleLocationCommand(player.ActiveCharacter, locationCommad.Value, args);
                }
                else
                {                                                                       //остальные команды
                    HandleBaseCommand(player, args);
                }
            }
            else
            {
                HandleTextMessage(player, args);                                                //если там просто текст
            }
            SQLManager.Save(player.GetData());
        }

        private void HandlePlayerAction(Player player, PlayerAction action, RequestEventArgs args)
        {
            player.TakeAction(action);
            switch (player.CurrentState)
            {
                case PlayerState.CreatingNewChar:
                    Response(args.ClientInfo, "Персонажу нужно имя, введи его и получишь результат");
                    break;
                case PlayerState.DeletingChar:
                    Response(args.ClientInfo, "Кого убить?", buttons: GenerateCharList(player.GetData()));
                    break;
                case PlayerState.InMenu:
                    Response(args.ClientInfo, "Че хочу?", buttons: new[] { Buttons.CreateChar, Buttons.DeleteChar, Buttons.CharInfo });
                    break;
                case PlayerState.InGame:
                    ResumeGame(player.ActiveCharacter, args);
                    break;
                case PlayerState.WatchingCharInfo:
                    Response(args.ClientInfo, "Про кого рассказать?", buttons: GenerateCharList(player.GetData()));
                    break;
                default:
                    break;
            }
        }

        private void HandleCharacterAction(Character character, CharacterAction action, RequestEventArgs args)
        {
            character.TakeAction(action);
            ResumeGame(character, args);
        }

        private void HandleLocationCommand(Character character, LocationCommand command, RequestEventArgs args)
        {
            #region Shop
            if (command.IsShopCommand())
            {                                               //МАГАЗ
                if (character.CurrentLocation == CharacterLocation.Shop)
                {
                    switch (command)
                    {
                        case LocationCommand.Buyarmor:
                            if (character.TryBuyItem(SQLManager.GetItem(character.GetData().armor.Id + 1, ItemSlot.armor)))
                            {
                                Response(args.ClientInfo, "Поздравляю с покупкой наряда");
                                SQLManager.Save(character.GetData());
                            }
                            else
                            {
                                Response(args.ClientInfo, "Извини, но мы живем при капитализме, чтоб одеться, нужны бабки");
                            }
                            break;
                        case LocationCommand.Buyweapon:
                            if (character.TryBuyItem(SQLManager.GetItem(character.GetData().weapon.Id + 1, ItemSlot.weapon)))
                            {
                                Response(args.ClientInfo, "Поздравляю с покупкой средства самообороны");
                                SQLManager.Save(character.GetData());
                            }
                            else
                            {
                                Response(args.ClientInfo, "Мы живем при капитализме, чтобы быть в безопасности, нужны бабки");
                            }
                            break;
                        case LocationCommand.Buypotion:
                            if (character.TryBuyItem(SQLManager.GetItem(character.GetData().potion.Id + 1, ItemSlot.potion)))
                            {
                                Response(args.ClientInfo, "Поздравляю с покупкой алкоголя");
                                SQLManager.Save(character.GetData());
                            }
                            else
                            {
                                Response(args.ClientInfo, "Мы живем при капитализме, чтобы выпить, нужны бабки");
                            }
                            break;
                    }
                }
                else
                {
                    Response(args.ClientInfo, "Чтобы купить что-то ходи в ларёк");
                }
            }
            #endregion

            #region SchoolOrPPK

            else if (command.IsSchoolCommand())
            {                                               //ППК
                if (character.CurrentLocation == CharacterLocation.School)
                {
                    switch (command)
                    {
                        case LocationCommand.LearnStr:
                            if (character.TryLearn(characterstat.Str))
                            {
                                Response(args.ClientInfo, "Препод бьет тебя палкой по рукам и они становятся сильнее");
                                SQLManager.Save(character.GetData());
                            }
                            else
                            {
                                Response(args.ClientInfo, "Сорян, но у тебя нет денег, чтобы учиться");
                            }
                            break;
                        case LocationCommand.LearnAgi:
                            if (character.TryLearn(characterstat.Agi))
                            {
                                Response(args.ClientInfo, "Препод бьет тебя прутиком по ногам и они становится быстрее");
                            }
                            else
                            {
                                Response(args.ClientInfo, "Сорян, но у тебя нет денег, чтобы учиться");
                            }
                            break;
                        case LocationCommand.LearnIntel:
                            if (character.TryLearn(characterstat.Intel))
                            {
                                Response(args.ClientInfo, "Препод бьет тебя учебником по голове и она становится умнее");
                            }
                            else
                            {
                                Response(args.ClientInfo, "Сорян, но у тебя нет денег, чтобы учиться");
                            }
                            break;
                        case LocationCommand.LearnPhy:
                            if (character.TryLearn(characterstat.Phy))
                            {
                                Response(args.ClientInfo, "Препод бьет тебя доской по жепе и она становится крепче");
                            }
                            else
                            {
                                Response(args.ClientInfo, "Сорян, но у тебя нет денег, чтобы учиться");
                            }
                            break;
                    }
                    SQLManager.Save(character.GetData());
                }
                else
                {
                    Response(args.ClientInfo, "Чтобы учиться, ходи в ППК");
                }
            }
            #endregion

            #region Arena
            else if (command.IsArenaCommand())
            {
                if (character.CurrentLocation == CharacterLocation.Arena)
                {
                    var enemy = character.CurrentEnemy;
                    switch (command)
                    {
                        case LocationCommand.Attack:
                            enemy.TakeDamage(character.Damage);
                            character.TakeDamage(enemy.Damage);
                            Response(args.ClientInfo, $"ТЫ АТАКУЕШЬ \n{enemy.Name} ПОЛУЧАЕТ {character.Damage} УРОНА ПРЯМО В ГОЛОВУ");
                            if (enemy.IsDead)
                            {
                                Response(args.ClientInfo, $"{enemy.Name} ВМЕР\nТы получаешь {enemy.Reward} денег");
                                character.TakeReward(enemy.Reward);
                                character.TakeAction(CharacterAction.GoHome);
                                break;
                            }
                            Response(args.ClientInfo, $"{enemy.Name} АТАКУЕТ И ТЫ ПОЛУЧАЕШЬ {enemy.Damage} УРОНА ПО ЛИЦУ");
                            if (character.IsDead)
                            {
                                Response(args.ClientInfo, $"ТЫ ВМЕР \n{enemy.Name} ЛИКУЕТ \nЗемля тебе пухом, друг");
                                character.TakeAction(CharacterAction.GoHome);
                            }
                            break;
                        case LocationCommand.Defence:
                            Response(args.ClientInfo, $"{enemy.Name} АТАКУЕТ НО ТЫ БЛОКИРУЕШЬ, ТАК МОЖЕТ ПРОДОЛЖАТЬСЯ БЕСКОНЕЧНО...");
                            break;
                        case LocationCommand.Usepotion:
                            if (character.TryUsepotion())
                            {
                                Response(args.ClientInfo, $"Ты выпил {character.GetData().potion.Name}\nВроде полегчало");
                            }
                            else
                            {
                                Response(args.ClientInfo, "Ты пытался выпить зелье, но не смог его найти");
                            }
                            character.TakeDamage(enemy.Damage);
                            Response(args.ClientInfo, $"{enemy.Name} АТАКУЕТ И ТЫ ПОЛУЧАЕШЬ {enemy.Damage} УРОНА ПО ЛИЦУ");
                            break;
                    }
                }
                else
                {
                    Response(args.ClientInfo, "Бухать, прыгать и махать руками нужно на арене, брат");
                }
            }
            ResumeGame(character, args);
        }
        #endregion

        #region /start
        private void HandleBaseCommand(Player player, RequestEventArgs args)
        {
            switch (args.Command)
            {
                case GameCommand.Start:
                    if (player.CurrentState == PlayerState.Greetings)
                    {
                        Response(
                            args.ClientInfo,
                            "Привет, здарова.\nЯ хочу сыграть с тобой в игру\nДавай создадим персонажа",
                            buttons: new[] { Buttons.CreateChar }
                        );
                    }
                    else
                    {
                        Response(args.ClientInfo, "Куда вот ты стартуешь?\nМы уже начали, играй давай", buttons: new[] { Buttons.Menu });
                    }
                    break;
            }
        }
        #endregion

        //Как это назвать ёпт, надо думать -v-
        #region MessageGet
        private void HandleTextMessage(Player player, RequestEventArgs args)
        {
            switch (player.CurrentState)
            {
                case PlayerState.Greetings:
                    Response(args.ClientInfo, "Чтобы играть, нужен перс", buttons: new[] { Buttons.CreateChar });
                    break;
                case PlayerState.CreatingNewChar:
                    CreateNewCharForPlayer(player, args.Text);
                    Response(args.ClientInfo, "Персонаж успешно создан", buttons: new[] { Buttons.Play, Buttons.Menu });
                    break;
                case PlayerState.DeletingChar:
                    DeleteCharacter(args.Text, player);
                    if (player.GetData().characters.Count > 0)
                    {
                        Response(args.ClientInfo, "Персонаж успшно удален, вот остальные:", buttons: GenerateCharList(player.GetData()));
                        player.TakeAction(PlayerAction.CharInfo);
                    }
                    else
                    {
                        Response(args.ClientInfo, "Персонаж успшно удален, других нет, нужен новый", buttons: new[] { Buttons.CreateChar });
                    }
                    break;
                case PlayerState.WatchingCharInfo:
                    var characterData = GetCharacterByName(args.Text, player.GetData());
                    player.SetActiveCharacter(new Character(characterData));
                    Response(args.ClientInfo, characterData.ToString(), buttons: new[] { Buttons.Play, Buttons.Menu });
                    break;
                case PlayerState.InMenu:
                case PlayerState.InGame:
                default:
                    Response(
                        args.ClientInfo,
                        $"Для управления кнопки есть." +
                        $"\nНе надо мне текст сейчас отправлять, я не просил" +
                        $"\nЕсли что, вот твой текущий стейт: {player.CurrentState}"
                    );
                    break;
            }
        }
        #endregion

        private void ResumeGame(Character character, RequestEventArgs args)
        {
            var location = character.GetCurrentLocation();
            var description = $"Твои текущие статы:\nЗдоровье: {character.CurrentHealth} \n💥Урон: {character.Damage}\n{character.GetData()}\n\n{location.Description}";
            if (location is Arena)
            {
                description += $"\nТвой текущий противник: {character.CurrentEnemy.Name} \nЕго здоровье: {character.CurrentEnemy.CurrentHealth}❤️ \nЕго урон: {character.CurrentEnemy.Damage}🔥";
            }
            Response(
                args.ClientInfo,
                description,
                location is Arena ? character.CurrentEnemy.ImageUrl : location.ImageUrl,
                location.GetButtons()
            );
        }

        private void CreateNewCharForPlayer(Player player, string name)
        {
            var charId = BotCode.GenerateCharId();
            var character = CreateCharacter(charId, name, player.Id);
            player.SetActiveCharacter(character);
            SQLManager.Save(player.GetData());
        }

        private Character CreateCharacter(int id, string name, long ownerId)
        {
            var data = new CharacterData
            {
                Id = id,
                Name = name,
                OwnerId = ownerId,
                weapon = SQLManager.GetItem(1, ItemSlot.weapon),
                armor = SQLManager.GetItem(1, ItemSlot.armor),
                potion = SQLManager.GetItem(1, ItemSlot.potion)
            };
            SQLManager.CreateNewCharacter(data);
            return new Character(data);
        }

        private void DeleteCharacter(string name, Player player)
        {
            var data = GetCharacterByName(name, player.GetData());
            player.RemoveCharacter(name);
            SQLManager.DeleteCharacter(data.Id);
        }

        private CharacterData GetCharacterByName(string name, PlayerData playerData)
        {
            var data = playerData.characters.FirstOrDefault(c => c.Name == name);
            if (data == null)
            {
                data = SQLManager.GetCharsForUser(playerData.Id).First(c => c.Name == name);
            }
            return data;
        }

        private List<InlineKeyboardButton> GenerateCharList(PlayerData playerData)
        {
            var result = new List<InlineKeyboardButton>();
            foreach (var charData in playerData.characters)
            {
                result.Add(InlineKeyboardButton.WithCallbackData(charData.Name, charData.Name));
            }
            return result;
        }

        private void Response(ClientInfo clientInfo, string text, string image = null, IEnumerable<InlineKeyboardButton> buttons = null)
        {
            var response = new ResponseEventArgs
            {
                ClientInfo = clientInfo,
                TextMessage = text,
                ImageUrl = image,
                Buttons = buttons
            };
            OnReadyToResponse.Invoke(this, response);
        }
    }
}
