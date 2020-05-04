﻿using System;
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
        public interface ICombatUnit
        {
            public int MaxHealthPoints { get; set; }
            public int HealthPoints { get; }
            public int MaxMoralePoints { get; set; }
            public int MoralePoints { get; }
            public int MaxSpecialPoints { get; set; }
            public int SpecialPoints { get; }
            public int Initiative { get; set; }
            public UnitState State { get; }
            public Dictionary<string, Action<ICombatUnit>> Perks { get; }
            public void ApplyHpDamage(int dmg);
            public void ApplyMpDamage(int dmg);
        }

        public class PlayerCombatUnit: ICombatUnit
        {
            public int MaxHealthPoints { get; set; }
            public int HealthPoints { get; private set; }
            public int MaxMoralePoints { get; set; }
            public int MoralePoints { get; private set; }
            public int MaxSpecialPoints { get; set; }
            public int SpecialPoints { get; private set; }
            public int Initiative { get; set; }
            public UnitState State { get; private set; }
            public Dictionary<string, Action<ICombatUnit>> Perks { get; private set; }
            
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

            public PlayerCombatUnit(Characters.PlayerChar character)
            {
                MaxHealthPoints = character.HealthPoints;
                HealthPoints = MaxHealthPoints;
                MaxMoralePoints = character.MoralePoints;
                MoralePoints = MaxMoralePoints;
                MaxSpecialPoints = character.SpecialPoints;
                SpecialPoints = MaxSpecialPoints;
                Initiative = character.Initiative;
                State = UnitState.Fine;
                Perks = character.CharClass.Perks;
            }

            public void CastPerk(string perkName, ICombatUnit target)
            {
                Perks[perkName](target);
            }
        }
    }
}