using System.Collections.Generic;

namespace Corovany.Logic
{
    public class CharClasses
    {
        public interface ICharClass
        {
            public static List<CharacterCore.Perk> Perks;
            public static CharacterCore.CharacterClass Class;
        }
        
        public class Human : ICharClass
        {
            private static readonly List<CharacterCore.Perk> Perks = new List<CharacterCore.Perk>()
            {
                new CharacterCore.Perk("Power Blade", "human_skill_power_blade",
                    $"Powerful strike that deals 50HP dmg",
                    10, 0, 1, unit =>
                {
                    unit.ApplyHpDamage(50);
                }),
                new CharacterCore.Perk("Fire Blade", "human_skill_fire_blade",
                    $"Fiery attack that deals 30HP dmg and 20MP dmg",
                    10, 0, 1, unit =>
                    {
                        unit.ApplyHpDamage(30);
                        unit.ApplyMpDamage(20);
                    })
            };
            public static readonly CharacterCore.CharacterClass Class = new CharacterCore.CharacterClass("Human", 
                100, 100, 100, 4, Perks);
        }
    }
}