using System;
using System.Collections.Generic;

namespace Corovany.logic
{
    public class CombatCore
    {
        public enum UnitState
        {
            Fine = 0,
            Escaped = 1,
            Dead = 2
        }
        public interface ICombatUnitPattern
        {
            public int HealthPoints { get; }
            public int MoralePoints { get; }
            public int SpecialPoints { get; }
            public int Initiative { get; set; }
            public UnitState State { get; }
            public Dictionary<CharacterCore.Perk, int> Cooldown { get; }
            public void ApplyHpDamage(int dmg);
            public void ApplyMpDamage(int dmg);
            public void HealHp(int hp);
            public void HealMp(int hp);
        }

        public class PlayerCombatUnit: ICombatUnitPattern
        {
            public CharacterCore.Character Character { get; set; }
            public int HealthPoints { get; private set; }
            public int MoralePoints { get; private set; }
            public int SpecialPoints { get; private set; }
            public int Initiative { get; set; }
            public UnitState State { get; private set; }
            public Dictionary<CharacterCore.Perk, int> Cooldown { get; } = new Dictionary<CharacterCore.Perk, int>();

            public void ApplyHpDamage(int dmg)
            {
                HealthPoints-= dmg;
                if (HealthPoints <= 0)
                    State = UnitState.Dead;
            }

            public void ApplyMpDamage(int dmg)
            {
                MoralePoints-= dmg;
                if (MoralePoints <= 0 && State != UnitState.Dead)
                    State = UnitState.Escaped;
            }

            public void HealHp(int hp)
            {
                HealthPoints = HealthPoints + hp >= Character.HealthPoints ? Character.HealthPoints : HealthPoints + hp;
            }
            
            public void HealMp(int mp)
            {
                MoralePoints = MoralePoints + mp >= Character.MoralePoints ? Character.MoralePoints : MoralePoints + mp;
            }

            private void GenerateCooldown()
            {
                foreach (var perk in Character.CharClass.Perks)
                    Cooldown.Add(perk, 0);
            }

            public PlayerCombatUnit(CharacterCore.Character character)
            {
                Character = character;
                HealthPoints = Character.HealthPoints;
                MoralePoints = Character.MoralePoints;
                SpecialPoints = Character.SpecialPoints;
                Initiative = Character.Initiative;
                State = UnitState.Fine;
                GenerateCooldown();
            }

            public void CastPerk(CharacterCore.Perk perk, ICombatUnitPattern target)
            {
                if (perk.LevelToUnlock <= Character.Level && Cooldown[perk]==0)
                {
                    perk.Ability(target);
                    Cooldown[perk] = perk.Cooldown;
                }
                Initiative += 100;
            }
        }
    }
}