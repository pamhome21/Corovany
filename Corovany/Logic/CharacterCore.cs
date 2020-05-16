using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Corovany.Logic
{
    public class CharacterCore
    {
        private const double LvlFactor = 0.66;
        public class CharacterClass
        {
            public string Name { get; }
            public int HealthPoints { get; }
            public int MoralePoints { get; }
            public int SpecialPoints { get; }
            public int Initiative { get; }
            public List<Perk> Perks { get; }

            public CharacterClass(string name, int hp, int mp, int sp, int initiative,
                List<Perk> perks)
            {
                Name = name;
                HealthPoints = hp;
                MoralePoints = mp;
                SpecialPoints = sp;
                Initiative = initiative;
                Perks = perks;
            }
        }
        public class Character
        {
            public string Name { get; private set; } = "Noname";
            public CharacterClass CharClass { get; set; } 
            public int Level { get; private set; }
            public int Initiative { get; set; }
            public int HealthPoints { get; set; }
            public int MoralePoints { get; set; }
            public int SpecialPoints { get; set; }
            public string OwnerId { get; set; }

            public Character(string name, CharacterClass charClass, int level)
            {
                Rename(name);
                CharClass = charClass;
                Level = SetLevel(level);
                UpdateLvl();
            }
            
            public Character(string name, CharacterClass charClass, int level, GameCore.Player owner)
            {
                Rename(name);
                CharClass = charClass;
                Level = SetLevel(level);
                OwnerId = owner.Id;
                UpdateLvl();
            }

            public void Rename(string newName)
            {
                if (newName.Length>0)
                    Name = newName;
            }

            private int SetLevel(int level)
            {
                return level < 1 ? 1 : level;
            }

            public void LevelUp()
            {
                Level++;
            }

            public void UpdateLvl()
            {
                var isNotFirstLvl = Level > 1;
                Initiative = isNotFirstLvl ? (int)(CharClass.Initiative * Level * LvlFactor) : CharClass.Initiative;
                HealthPoints = isNotFirstLvl ? (int)(CharClass.HealthPoints * Level * LvlFactor) : CharClass.HealthPoints;
                MoralePoints = isNotFirstLvl ? (int)(CharClass.MoralePoints * Level * LvlFactor) : CharClass.MoralePoints;
                SpecialPoints = isNotFirstLvl ? (int)(CharClass.SpecialPoints * Level * LvlFactor) : CharClass.SpecialPoints;
            }
        }

        public class Perk
        {
            public string Name { get; }
            public int Cost { get; }
            public int Cooldown { get; }
            public int LevelToUnlock { get; }
            [JsonIgnore]
            public Action<CombatCore.ICombatUnitPattern> Ability { get; }

            public Perk(string name, int cost, int cd, int lvlToUnlock, Action<CombatCore.ICombatUnitPattern> ability)
            {
                Name = name;
                Cost = cost;
                Cooldown = cd;
                LevelToUnlock = lvlToUnlock;
                Ability = ability;
            }
        }
    }
}