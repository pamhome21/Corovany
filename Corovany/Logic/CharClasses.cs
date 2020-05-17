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
                    20, 2, 1, unit =>
                {
                    unit.ApplyHpDamage(50);
                }),
                new CharacterCore.Perk("Fire Blade", "human_skill_fire_blade",
                    $"Fiery attack that deals 30HP and 20MP dmg",
                    15, 0, 1, unit =>
                    {
                        unit.ApplyHpDamage(30);
                        unit.ApplyMpDamage(20);
                    }),
                new CharacterCore.Perk("Taunt", "skip_turn",
                    $"Taunts enemy (deals nothing)",
                    0,0,0, unit => {}
                    )
            };
            public static readonly CharacterCore.CharacterClass Class = new CharacterCore.CharacterClass("Human", 
                100, 50, 40, 4, Perks);
        }
        
        public class Magician : ICharClass
        {
            private static readonly List<CharacterCore.Perk> Perks = new List<CharacterCore.Perk>()
            {
                new CharacterCore.Perk("Boost Morale", "magician_skill_boost_morale",
                    $"Heals selected unit 20MP",
                    30, 0, 1, unit =>
                    {
                        unit.HealMp(20);
                    }),
                new CharacterCore.Perk("Healing Spell", "magician_skill_healing_lilly",
                    $"Heals selected unit 20HP",
                    15, 0, 1, unit =>
                    {
                        unit.HealHp(20);
                    }),
                new CharacterCore.Perk("Taunt", "skip_turn",
                    $"Taunts enemy (deals nothing)",
                    0,0,0, unit => {}
                )
            };
            public static readonly CharacterCore.CharacterClass Class = new CharacterCore.CharacterClass("Magician", 
                60, 60, 100, 8, Perks);
        }

        public class Elf : ICharClass
        {
            private static readonly List<CharacterCore.Perk> Perks = new List<CharacterCore.Perk>()
            {
                new CharacterCore.Perk("Shoot Arrows", "elf_skill_shoot_arrow",
                $"Shoot target with an arrow that deals 15HP and 5MP dmg",
                10, 0, 1, unit =>
                {
                    unit.ApplyHpDamage(15);
                    unit.ApplyMpDamage(5);
                }),
                new CharacterCore.Perk("Nature's Wrath", "elf_skill_roses",
                    $"Powerful spell that deals 30MP dmg",
                    15, 0, 1, unit =>
                    {
                        unit.ApplyMpDamage(30);
                    }),
                new CharacterCore.Perk("Taunt", "skip_turn",
                    $"Taunts enemy (deals nothing)",
                    0,0,0, unit => {}
                )
            };
            public static readonly CharacterCore.CharacterClass Class = new CharacterCore.CharacterClass("Elf", 
                50, 40, 30, 2, Perks);
        }

        public class Hobbit : ICharClass
        {
            private static readonly List<CharacterCore.Perk> Perks = new List<CharacterCore.Perk>()
            {
                new CharacterCore.Perk("Taunt", "skip_turn",
                    $"Taunts enemy (deals nothing)",
                    0,0,0, unit => {}
                )
            };
            public static readonly CharacterCore.CharacterClass Class = new CharacterCore.CharacterClass("Hobbit", 
                50, 30, 10, 5, Perks);
        }

        public class Ciclopus : ICharClass
        {
            private static readonly List<CharacterCore.Perk> Perks = new List<CharacterCore.Perk>()
            {
                new CharacterCore.Perk("Throw Rock", "ciclopus_skill_throw_rock",
                    $"Thows giant rock that deals 45HP dmg",
                    50, 0, 1, unit =>
                    {
                        unit.ApplyHpDamage(45);
                    }),
                new CharacterCore.Perk("Shout", "ciclopus_skill_decrease_morale",
                    $"Decreases 45MP of target",
                    50, 0, 1, unit =>
                    {
                        unit.ApplyMpDamage(45);
                    }),
                new CharacterCore.Perk("Taunt", "skip_turn",
                    $"Taunts enemy (deals nothing)",
                    0,0,0, unit => {}
                )
            };
            public static readonly CharacterCore.CharacterClass Class = new CharacterCore.CharacterClass("Ciclopus", 
                200, 200, 50, 20, Perks);
        }
    }
}