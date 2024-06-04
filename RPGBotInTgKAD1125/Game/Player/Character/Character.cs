using System;

namespace MyBot.Game
{
    public class Character
    {
        private CharacterData Data;
        private characterstateMachine StateMachine;
        private int currentHealth;
        private int maxHealth;
        private int damage;
        private bool ispotionUsedAlready = false;


        public Monster CurrentEnemy;

        public CharacterLocation CurrentLocation => StateMachine.CurrentState;
        public int Id => Data.Id;
        public int CurrentHealth => currentHealth;
        public int Damage => damage;
        public bool IsDead => currentHealth <= 0;

        public Character(CharacterData data)
        {
            Data = new CharacterData(data);
            StateMachine = new characterstateMachine(data.State);
            StateMachine.LocationChanged += OnLocationChanged;
            UpdateMaxHealth();
            UpdateDamage();
        }

        public void TakeReward(int value)
        {
            Data.Gold += value;
        }

        public Location GetCurrentLocation()
        {
            return CurrentLocation switch
            {
                CharacterLocation.Home => Locations.Home,
                CharacterLocation.Shop => Locations.Shop,
                CharacterLocation.School => Locations.School,
                CharacterLocation.Arena => Locations.Arena,
                _ => null
            };
        }

        public void UpdateMaxHealth()
        {
            maxHealth = 2 + Data.Phy * 2 + Data.Agi + Data.armor.Value;
        }

        public void ResetHP()
        {
            currentHealth = maxHealth;
            ispotionUsedAlready = false;
        }

        public void UpdateDamage()
        {
            damage = 1 + Data.Str + (Data.Agi / 2) + Data.weapon.Value;
        }

        public bool TryUsepotion()
        {
            if (!ispotionUsedAlready)
            {
                currentHealth += Data.potion.Value + (Data.Intel*3);
                ispotionUsedAlready = true;
                return true;
            }
            return false;
        }

        public void TakeDamage(int value)
        {
            currentHealth -= value;
        }

        public void TakeAction(CharacterAction action) => StateMachine.Act(action);

        public CharacterData GetData() => new CharacterData(Data);

        public bool TryBuyItem(ItemData data)
        {
            if (Data.Gold >= data.Cost)
            {
                Data.Gold -= data.Cost;
                switch (data.Slot)
                {
                    case ItemSlot.weapon:
                        Data.weapon = new ItemData(data);
                        UpdateDamage();
                        break;
                    case ItemSlot.armor:
                        Data.armor = new ItemData(data);
                        UpdateMaxHealth();
                        break;
                    case ItemSlot.potion:
                        Data.potion = new ItemData(data);
                        break;
                }
                return true;
            }
            return false;
        }

        private void OnLocationChanged(object sender, CharacterLocation state)
        {
            ResetHP();
            if (state == CharacterLocation.Arena)
            {
                CurrentEnemy = Monsters.GetRandom();
                CurrentEnemy.ResetHealth();
                Locations.Arena = new Arena();
            }
        }
        public bool TryLearn(characterstat stat)
        {
            if (Data.Gold > 0)
            {
                Data.Gold--;
                switch (stat)
                {
                    case characterstat.Str:
                        Data.Str++;
                        UpdateDamage();
                        break;
                    case characterstat.Agi:
                        Data.Agi++;
                        UpdateDamage();
                        UpdateMaxHealth();
                        break;
                    case characterstat.Phy:
                        Data.Phy++;
                        UpdateMaxHealth();
                        break;
                    case characterstat.Intel:
                        Data.Intel++;
                        break;
                }
                Data.Level++;
                return true;
            }
            return false;
        }
    }

    public enum characterstat
    {
        Str,
        Agi,
        Phy,
        Intel
    }
}
