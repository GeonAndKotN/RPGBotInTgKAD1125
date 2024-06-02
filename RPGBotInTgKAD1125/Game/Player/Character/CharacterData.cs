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
        public ItemData Weapon { get; set; }
        public ItemData Armor { get; set; }
        public ItemData Potion { get; set; }
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
            Weapon = new ItemData(data.Weapon);
            Armor = new ItemData(data.Armor);
            Potion = new ItemData(data.Potion);
        }

        
        //Эта реализация нужна для более удобного дебага.
        public override string ToString()
        {
            // StringBuilder это - 
            var builder = new StringBuilder();
            builder.Append("Имя: ");
            builder.Append(Name);
            builder.Append("\nУровень: ");
            builder.Append(Level);
            builder.Append("\nСтойкость: ");
            builder.Append(Phy);
            builder.Append("\nСила: ");
            builder.Append(Str);
            builder.Append("\nЛовкость: ");
            builder.Append(Agi);
            builder.Append("\nИнтеллект: ");
            builder.Append(Intel);
            builder.Append("\nРубли: ");
            builder.Append(Gold);
            builder.Append("\nОружие: ");
            builder.Append(Weapon.Name);
            builder.Append("\nОдежда: ");
            builder.Append(Armor.Name);
            builder.Append("\nЗелье: ");
            builder.Append(Potion.Name);
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
