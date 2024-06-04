using System.Collections.Generic;
using System.Linq;

namespace MyBot.Game
{
    public class PlayerData
    {
        public PlayerData() { }

        /// <summary>
        /// Копируем данные, чтобы не было ситуации с их внезапным изменением
        /// Можно было бы и через Clone() сделать, но нет разницы
        /// </summary>
        /// <param name="data">Данные, которые копируем</param>
        public PlayerData(PlayerData data)
        {
            Id = data.Id;
            State = data.State;
            characters = data.characters.Select(cd => new CharacterData(cd)).ToList();
        }

        public override bool Equals(object obj)
        {
            if (obj is PlayerData)
            {
                var data = obj as PlayerData;
                return Id == data.Id;
            }
            return false;
        }

        public long Id { get; set; }
        public PlayerState State { get; set; } = PlayerState.Greetings;
        public List<CharacterData> characters { get; set; } = new List<CharacterData>();
    }
}
