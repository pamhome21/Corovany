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
            private static readonly List<CharacterCore.Perk> Perks = new List<CharacterCore.Perk>()
            {
                new CharacterCore.Perk("Kek", 10, 0, 1, unit =>
                {
                    unit.ApplyHpDamage(50);
                })
            };
            public static readonly CharacterCore.CharacterClass Class = new CharacterCore.CharacterClass("Testificate", 
                100, 100, 100, 4, Perks);
        }
        public class TestificateBotCl : ICharClass
        {
            private static readonly List<CharacterCore.Perk> Perks = new List<CharacterCore.Perk>()
            {
                new CharacterCore.Perk("Kek", 10, 0, 1, unit =>
                {
                    unit.ApplyHpDamage(20);
                })
            };
            public static readonly CharacterCore.CharacterClass Class = new CharacterCore.CharacterClass("Testificate", 
                50, 100, 100, 10, Perks);
        }
        public class SlowpokeBotCl : ICharClass
        {
            private static readonly List<CharacterCore.Perk> Perks = new List<CharacterCore.Perk>()
            {
                new CharacterCore.Perk("Kek", 10, 0, 1, unit =>
                {
                    unit.ApplyHpDamage(20);
                })
            };
            public static readonly CharacterCore.CharacterClass Class = new CharacterCore.CharacterClass("Testificate", 
                100, 100, 100, 20, Perks);
        }
        
        public class SlowpokeCl : ICharClass
        {
            private static readonly List<CharacterCore.Perk> Perks = new List<CharacterCore.Perk>()
            {
                new CharacterCore.Perk("Kek", 10, 0, 1, unit =>
                {
                    unit.ApplyHpDamage(100);
                })
            };
            public static readonly CharacterCore.CharacterClass Class = new CharacterCore.CharacterClass("Testificate", 
                100, 100, 100, 25, Perks);
        }
    }
}