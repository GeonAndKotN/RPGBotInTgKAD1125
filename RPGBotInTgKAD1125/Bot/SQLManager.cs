using MyBot.Game;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace MyBot.DataBase
{
    public class SQLManager
    {
        const string DbUser = "student";
        const string DbName = "BotTg1125KAD";
        const string DbPass = "student";

        //server=192.168.200.13;user=student;password=student;database=BotTg1125KAD чтобы в базе ппк было всё хорошо, надеюсь тут MySQL Workbench не будет кусаться

        public readonly string Connectionstring =
                $"server=192.168.200.13;" +
                $"user={DbUser};" +
                $"database={DbName};" +
                $"password={DbPass}";

        public SQLManager() { }

        //сохранялочки
        #region Save
        public void Save(PlayerData data)
        {
            try
            {
                using (var connection = new MySqlConnection(Connectionstring))
                {
                    connection.Open();
                    var saveCommand = $"UPDATE `{DbName}`.`users` SET `State` = {(int)data.State} WHERE User_id = {data.Id}";
                    using (var reader = new MySqlCommand(saveCommand, connection).ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            for (int i = 0; i < data.characters.Count; i++)
                            {
                                Save(data.characters[i]);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Не удалось сохраниться F1, вот ошибка: {e.Message}");
            }
        }

        public void Save(CharacterData data)
        {
            try
            {
                using (var connection = new MySqlConnection(Connectionstring))
                {
                    connection.Open();
                    var updateCharCommand = $"UPDATE {DbName}. characters SET " +
                    $"Name = '{data.Name}', " +
                    $"Level = {data.Level}, " +
                    $"Phy = {data.Phy}, " +
                    $"Str = {data.Str}, " +
                    $"Agi = {data.Agi}, " +
                    $"Intel = {data.Intel}, " +
                    $"weapons_weapon_id = {data.weapon.Id}, " +
                    $"armors_armor_id = {data.armor.Id}, " +
                    $"potions_potion_id = {data.potion.Id}, " +
                    $"State = {(int)data.State}, " +
                    $"Gold = {data.Gold} " +
                    $"WHERE Character_id = {data.Id}";
                    var reader = new MySqlCommand(updateCharCommand, connection).ExecuteReader();
                    reader.Read();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Не удалось сохраниться F2, вот ошибка: {e.Message}");
            }
        }

        public void SaveNewPlayer(PlayerData data)
        {
            try
            {
                using (var connection = new MySqlConnection(Connectionstring))
                {
                    connection.Open();
                    var command = $"Insert into users Values ({data.Id},{(int)data.State})";
                    var reader = new MySqlCommand(command, connection).ExecuteReader();
                    reader.Read();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Не удалось создать игрока вот ошибка F1: {e.Message}");
            }
        }

        public void CreateNewCharacter(CharacterData data)
        {
            try
            {
                using (var connection = new MySqlConnection(Connectionstring))
                {
                    connection.Open();
                    var command = $"Insert into characters Values (" +
                        $"{data.Id}," +
                        $"{data.OwnerId}," +
                        $"'{data.Name}'," +
                        $"{data.Level}," +
                        $"{data.Phy}," +
                        $"{data.Str}," +
                        $"{data.Agi}," +
                        $"{data.Intel}," +
                        $"{(int)data.State}," +
                        $"{data.Gold}," +
                        $"{data.armor.Id}," +
                        $"{data.weapon.Id}," +
                        $"{data.potion.Id})";
                    var reader = new MySqlCommand(command, connection).ExecuteReader();
                    reader.Read();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Не удалось создать перса вот ошибка F2: {e.Message}");
            }
        }
        #endregion

        //Загрузочки
        #region Load
        public PlayerData LoadPlayerData(long id)
        {
            try
            {
                var data = new PlayerData { Id = id };
                var loadUserCommand = $"SELECT * FROM users WHERE User_id = {id}";
                using (var connection = new MySqlConnection(Connectionstring))
                {
                    connection.Open();
                    using (var reader = new MySqlCommand(loadUserCommand, connection).ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            data.State = (PlayerState)reader.GetInt32("State");
                            data.characters = GetCharsForUser(id);
                        }
                        else
                        {
                            SaveNewPlayer(data);
                        }
                    }
                    connection.Close();
                }
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public List<CharacterData> GetCharsForUser(long ownerId)
        {
            var result = new List<CharacterData>();
            try
            {
                using (var connection = new MySqlConnection(Connectionstring))
                {
                    connection.Open();
                    var loadcharactersCommand = $"SELECT * FROM characters WHERE Owner = {ownerId}";
                    using (var reader = new MySqlCommand(loadcharactersCommand, connection).ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var data = new CharacterData
                            {
                                Id = reader.GetInt32("Character_id"),
                                OwnerId = reader.GetInt64("Owner"),
                                Name = reader.GetString("Name"),
                                Level = reader.GetInt32("Level"),
                                Phy = reader.GetInt32("Phy"),
                                Str = reader.GetInt32("Str"),
                                Agi = reader.GetInt32("Agi"),
                                Intel = reader.GetInt32("Intel"),
                                Gold = reader.GetInt32("Gold"),
                                State = (CharacterLocation)reader.GetInt32("State"),
                                weapon = GetItem(reader.GetInt32("weapons_weapon_id"), ItemSlot.weapon),
                                armor = GetItem(reader.GetInt32("armors_armor_id"), ItemSlot.armor),
                                potion = GetItem(reader.GetInt32("potions_potion_id"), ItemSlot.potion),
                            };
                            result.Add(data);
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Не удалось загрузать персонажей, вот ошибка: {e.Message}");
            }
            return result;
        }

        public CharacterData GetCharacter(int id)
        {
            try
            {
                using (var connection = new MySqlConnection(Connectionstring))
                {
                    connection.Open();
                    var loadcharactersCommand = $"SELECT * FROM characters WHERE Character_id = {id}";
                    using (var reader = new MySqlCommand(loadcharactersCommand, connection).ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var data = new CharacterData
                            {
                                Id = reader.GetInt32("Character_id"),
                                OwnerId = reader.GetInt64("Owner"),
                                Name = reader.GetString("Name"),
                                Level = reader.GetInt32("Level"),
                                Phy = reader.GetInt32("Phy"),
                                Str = reader.GetInt32("Str"),
                                Agi = reader.GetInt32("Agi"),
                                Intel = reader.GetInt32("Intel"),
                                Gold = reader.GetInt32("Gold"),
                                State = (CharacterLocation)reader.GetInt32("State"),
                                weapon = GetItem(reader.GetInt32("weapons_weapon_id"), ItemSlot.weapon),
                                armor = GetItem(reader.GetInt32("armors_armor_id"), ItemSlot.armor),
                                potion = GetItem(reader.GetInt32("potions_potion_id"), ItemSlot.potion),
                            };
                            return data;
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Не удалось загрузить перса, вот ошибка: {e.Message}");
            }
            return null;
        }

        public ItemData GetItem(int id, ItemSlot slot)
        {
            var result = new ItemData { Id = id };
            try
            {
                using (var connection = new MySqlConnection(Connectionstring))
                {
                    connection.Open();
                    var loadItemCommand = $"SELECT * FROM {slot}s WHERE {slot}_id = {id}";
                    using (var reader = new MySqlCommand(loadItemCommand, connection).ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result.Name = reader.GetString("Name");
                            result.Value = reader.GetInt32("Value");
                            result.Cost = reader.GetInt32("Cost");
                            result.Slot = slot;
                        }
                    }
                    connection.Close();
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Не удалось загрузить предмет, вот ошибка: {e.Message}");
            }
            return null;
        }
        #endregion

        public void DeleteCharacter(int id)
        {
            try
            {
                using (var connection = new MySqlConnection(Connectionstring))
                {
                    connection.Open();
                    var command = $"DELETE FROM characters WHERE Character_id = {id}";
                    using (var reader = new MySqlCommand(command, connection).ExecuteReader())
                    {
                        reader.Read();
                    }
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Не удалось удалить перса, вот ошибка F1: {e.Message}");
                throw;
            }
        }
    }
}