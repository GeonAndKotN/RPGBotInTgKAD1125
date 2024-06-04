using System.Text;

namespace MyBot.Game
{
    public class CharacterData
    {
        public int Id { get; set; }
        public long OwnerId { get; set; }
        public string Name { get; set; }
        public int Level { get; set; } = 1;
        public int Phy { get; set; } = 1;
        public int Str { get; set; } = 1;
        public int Agi { get; set; } = 1;
        public int Intel { get; set; } = 1;
        public int Gold { get; set; } = 3;
        public CharacterLocation State { get; set; }
        public ItemData weapon { get; set; }
        public ItemData armor { get; set; }
        public ItemData potion { get; set; }
        //для сохранения состояний битвы
        public int? CurrentEnemy { get; set; } = null;

        public CharacterData() { }

        /// <summary>
        /// Копируем данные, чтобы не было ситуации с их внезапным изменением
        /// </summary>
        /// <param name="data">Данные, которые копируем</param>
        public CharacterData(CharacterData data)
        {
            Id = data.Id;
            OwnerId = data.OwnerId;
            Name = data.Name;
            Level = data.Level;
            Phy = data.Phy;
            Str = data.Str;
            Agi = data.Agi;
            Intel = data.Intel;
            Gold = data.Gold;
            State = data.State;
            weapon = new ItemData(data.weapon);
            armor = new ItemData(data.armor);
            potion = new ItemData(data.potion);
        }

        
        //Эта реализация нужна для более удобного дебага.
        public override string ToString()
        {
            // stringBuilder это - 
            var builder = new StringBuilder();
            builder.Append("💬Имя: ");
            builder.Append(Name);
            builder.Append("\n🧠Уровень: ");
            builder.Append(Level);
            builder.Append("\n📙Стойкость: ");
            builder.Append(Phy);
            builder.Append("\n📕Сила: ");
            builder.Append(Str);
            builder.Append("\n📗Ловкость: ");
            builder.Append(Agi);
            builder.Append("\n📘Интеллект: ");
            builder.Append(Intel);
            builder.Append("\n💳Рубли: ");
            builder.Append(Gold);
            builder.Append("\n🗡Оружие: ");
            builder.Append(weapon.Name);
            builder.Append("\n🪖Одежда: ");
            builder.Append(armor.Name);
            builder.Append("\n🍷Зелье: ");
            builder.Append(potion.Name);
            return builder.ToString();
        }

        //Для различных проверок
        public override bool Equals(object obj)
        {
            if (obj is CharacterData)
            {
                var data = obj as CharacterData;
                return Id == data.Id &&
                    Name == data.Name &&
                    OwnerId == data.OwnerId;
            }
            return false;
        }
    }
}
