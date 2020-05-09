using System.Collections.Generic;

namespace Corovany.logic
{
    public class CharClasses
    {
        public interface ICharClass
        {
            public static List<CharacterCore.Perk> Perks;
            public static CharacterCore.CharacterClass Class;
        }
        
        public class TestificateCl : ICharClass
        {
            public static List<CharacterCore.Perk> Perks = new List<CharacterCore.Perk>()
            {
                new CharacterCore.Perk("Kek", 10, 0, 1, unit =>
                {
                    unit.ApplyHpDamage(50);
                })
            };
            public static CharacterCore.CharacterClass Class = new CharacterCore.CharacterClass("Testificate", 
                100, 100, 100, 4, Perks);
        }
    }
}