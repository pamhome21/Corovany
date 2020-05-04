using System;
using System.Collections.Generic;

namespace Corovany.logic
{
    public class Characters
    {
        private const double LvlFactor = 0.66;
        public class CharacterClass
        {
            public string Name { get; }
            public int HealthPoints { get; }
            public int MoralePoints { get; }
            public int SpecialPoints { get; }
            public int Initiative { get; }
            public Dictionary<string, Action<CombatCore.ICombatUnit>> Perks { get; }

            public CharacterClass(string name, int hp, int mp, int sp, int initiative,
                Dictionary<string, Action<CombatCore.ICombatUnit>> perks)
            {
                Name = name;
                HealthPoints = hp;
                MoralePoints = mp;
                SpecialPoints = sp;
                Initiative = initiative;
                Perks = perks;
            }
        }
        public class PlayerChar
        {
            public string Name { get; private set; }
            public CharacterClass CharClass { get; set; } 
            public int Level { get; set; }
            public int Initiative { get; set; }
            public int HealthPoints { get; set; }
            public int MoralePoints { get; set; }
            public int SpecialPoints { get; set; }

            public PlayerChar(string name, CharacterClass charClass, int level)
            {
                Name = name;
                CharClass = charClass;
                Level = level;
                UpdateLvl();
            }

            public void Rename(string newName)
            {
                if (newName.Length>0)
                    Name = newName;
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
    }
}