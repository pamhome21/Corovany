using System;
using System.Collections.Generic;
using System.Linq;

namespace Corovany.Logic
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
            public Dictionary<string, int> Cooldown { get; }
            public void ApplyHpDamage(int dmg);
            public void ApplyMpDamage(int dmg);
            public void HealHp(int hp);
            public void HealMp(int hp);
        }

        /// <summary>
        ///     Structure implementing every unit on the field.
        ///     Here are defined basic units' operations methods
        /// </summary>
        public class PlayerCombatUnit: ICombatUnitPattern
        {
            
            public CharacterCore.Character Character { get; set; }
            public int HealthPoints { get; private set; }
            public int MoralePoints { get; private set; }
            public int SpecialPoints { get; private set; }
            public int Initiative { get; set; }
            public UnitState State { get; private set; }
            public Dictionary<string, int> Cooldown { get; } = new Dictionary<string, int>();

            public void ApplyHpDamage(int dmg)
            {
                HealthPoints = HealthPoints - dmg <= 0 ? 0 : HealthPoints - dmg;
                if (HealthPoints <= 0)
                    State = UnitState.Dead;
            }

            public void ApplyMpDamage(int dmg)
            {
                MoralePoints = MoralePoints - dmg <= 0 ? 0 : MoralePoints - dmg;
                if (MoralePoints <= 0 && State != UnitState.Dead)
                    State = UnitState.Escaped;
            }

            public void HealHp(int hp)
            {
                HealthPoints = HealthPoints + hp >= Character.HealthPoints 
                    ? Character.HealthPoints 
                    : HealthPoints + hp;
            }
            
            public void HealMp(int mp)
            {
                MoralePoints = MoralePoints + mp >= Character.MoralePoints 
                    ? Character.MoralePoints 
                    : MoralePoints + mp;
            }

            public void IncreaseSp(int sp)
            {
                SpecialPoints = SpecialPoints + sp >= Character.SpecialPoints
                    ? Character.SpecialPoints
                    : SpecialPoints + sp;
            }

            public void DecreaseSp(int sp)
            {
                SpecialPoints = SpecialPoints - sp <= 0 ? 0 : SpecialPoints - sp;
            }

            private void GenerateCooldown()
            {
                foreach (var perk in Character.CharClass.Perks)
                    Cooldown.Add(perk.Name, 0);
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
                perk.Ability(target);
                Cooldown[perk.Name] = perk.Cooldown;
                DecreaseSp(perk.Cost);
                Initiative += 100;
            }

            public bool CanCastPerk(CharacterCore.Perk perk)
            {
                return perk.LevelToUnlock <= Character.Level
                        && Cooldown[perk.Name] == 0 && perk.Cost <= SpecialPoints;
            }

            public void DecreaseCooldown()
            {
                var keys = new List<string>(Cooldown.Keys);
                foreach (var key in keys)
                {
                    if (Cooldown[key]>0)
                    {
                        Cooldown[key]--;
                    }
                }
            }
        }
    }
}