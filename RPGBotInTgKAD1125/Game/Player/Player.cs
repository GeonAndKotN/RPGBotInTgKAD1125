using System.Linq;

namespace MyBot.Game
{
    public class Player
    {
        private PlayerData Data;
        private PlayerStateMachine StateMachine;
        public Character ActiveCharacter { get; private set; }

        public PlayerState CurrentState => StateMachine.CurrentState;
        public long Id => Data.Id;

        public Player(PlayerData data)
        {
            Data = new PlayerData(data);
            StateMachine = new PlayerStateMachine(data.State);
            if (data.characters.Any())
            {
                SetActiveCharacter(new Character(data.characters[0]));
            }
        }

        public PlayerData GetData() => new PlayerData(Data);

        public void TakeAction(PlayerAction action) => StateMachine.Act(action);

        public void SetActiveCharacter(Character character)
        {
            if (!Data.characters.Contains(character.GetData()))
            {
                Data.characters.Add(character.GetData());
            }
            ActiveCharacter = character;
        }

        public void RemoveCharacter(string name)
        {
            Data.characters.Remove(Data.characters.First(c => c.Name == name));
        }
    }
}
