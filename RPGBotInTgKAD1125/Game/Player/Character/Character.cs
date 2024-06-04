using System;

namespace MyBot.Game
{
    public class Character
    {
        private CharacterData Data;
        private CharacterStateMachine StateMachine;
        private int currentHealth;
        private int maxHealth;
        private int damage;
        private bool isPotionUsedAlready = false;


        public Monster CurrentEnemy;

        public CharacterLocation CurrentLocation => StateMachine.CurrentState;
        public int Id => Data.Id;
        public int CurrentHealth => currentHealth;
        public int Damage => damage;
        public bool IsDead => currentHealth <= 0;

        public Character(CharacterData data)
        {
            Data = new CharacterData(data);
            StateMachine = new CharacterStateMachine(data.State);
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
            maxHealth = 3 + Data.Phy * 2 + Data.Agi + Data.Armor.Value;
        }

        public void ResetHP()
        {
            currentHealth = maxHealth;
            isPotionUsedAlready = false;
        }

        public void UpdateDamage()
        {
            damage = 2 + Data.Str + (Data.Agi / 2) + Data.Weapon.Value;
        }

        public bool TryUsePotion()
        {
            if (!isPotionUsedAlready)
            {
                currentHealth += Data.Potion.Value + Data.Intel;
                isPotionUsedAlready = true;
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
                    case ItemSlot.Weapon:
                        Data.Weapon = new ItemData(data);
                        UpdateDamage();
                        break;
                    case ItemSlot.Armor:
                        Data.Armor = new ItemData(data);
                        UpdateMaxHealth();
                        break;
                    case ItemSlot.Potion:
                        Data.Potion = new ItemData(data);
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
        public bool TryLearn(CharacterStat stat)
        {
            if (Data.Gold > 0)
            {
                Data.Gold--;
                switch (stat)
                {
                    case CharacterStat.Str:
                        Data.Str++;
                        UpdateDamage();
                        break;
                    case CharacterStat.Agi:
                        Data.Agi++;
                        UpdateDamage();
                        UpdateMaxHealth();
                        break;
                    case CharacterStat.Phy:
                        Data.Phy++;
                        UpdateMaxHealth();
                        break;
                    case CharacterStat.Intel:
                        Data.Intel++;
                        break;
                }
                Data.Level++;
                return true;
            }
            return false;
        }
    }

    public enum CharacterStat
    {
        Str,
        Agi,
        Phy,
        Intel
    }
}
