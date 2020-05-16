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
                new CharacterCore.Perk("Power Blade", "human_skill_power_blade.png",
                    $"Powerful strike that deals 50HP damage",
                    10, 0, 1, unit =>
                {
                    unit.ApplyHpDamage(50);
                })
            };
            public static readonly CharacterCore.CharacterClass Class = new CharacterCore.CharacterClass("Human", 
                100, 100, 100, 4, Perks);
        }
    }
}