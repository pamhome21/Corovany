using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Corovany.logic
{
    public class GameCore
    {
        private const int MoneyOnStart = 500;
        private const int MaxActiveChars = 3;
        public class Player
        {
            public string Name { get; set; }
            public int Gold { get; set; }
            public int Shards { get; set; }
            public List<CharacterCore.Character> PlayerChars { get; set; }
            public CharacterCore.Character[] CurrentChars { get; set; }

            public Player(string name)
            {
                Name = name;
                Gold = MoneyOnStart;
                Shards = 0;
                PlayerChars = new List<CharacterCore.Character>();
                CurrentChars = new CharacterCore.Character[MaxActiveChars];
            }
        }

        public class Enemy
        {
            public List<CharacterCore.Character> EnemyChars { get; set; }

            public Enemy()
            {
                EnemyChars = new List<CharacterCore.Character>();
            }
        }

        public class Game
        {
            public Dictionary<string, Player> Players { get; set; }
            public List<Enemy> Enemies { get; set; }
            public List<CombatCore.PlayerCombatUnit> Units { get; set; }
            public Queue<CombatCore.PlayerCombatUnit> UnitTurnQueue { get; set; }
            public Action<string> ReportInfo { get; set; }
            public CombatCore.PlayerCombatUnit CurrentUnit { get; set; }
            public Dictionary<string,CombatCore.PlayerCombatUnit> AvailableTargets { get; set; }
            public Dictionary<string,CharacterCore.Perk> AvailablePerks { get; set; }

            public Game(Action<string> reportInfo)
            {
                Players = new Dictionary<string, Player>();
                Enemies = new List<Enemy>();
                Units = new List<CombatCore.PlayerCombatUnit>();
                ReportInfo = reportInfo;
            }

            public void GetUnitFromQueue()
            {
                var counter = 0;
                while (counter == 0 || CurrentUnit.Character.Owner == null && counter < Units.Count-1)
                {
                    CurrentUnit = UnitTurnQueue.Dequeue();
                    counter++;
                    if (CurrentUnit.Character.Owner != null)
                    {
                        ReportInfo($"Ход персонажа {CurrentUnit.Character.Name} игрока {CurrentUnit.Character.Owner.Name}");
                    }
                    AvailableTargets = Units
                        .Where(unit => unit!=CurrentUnit)
                        .ToDictionary(unit => unit.Character.Name);
                    AvailablePerks = CurrentUnit.Character.CharClass.Perks
                        .ToDictionary(perk => perk.Name);
                    UnitTurnQueue.Enqueue(CurrentUnit);
                }
            }
            
            public void FillQueueWithUnits()
            {
                UnitTurnQueue = new Queue<CombatCore.PlayerCombatUnit>();
                Units.Sort(new UnitComparer());
                foreach (var unit in Units)
                {
                    UnitTurnQueue.Enqueue(unit);
                }
                ReportInfo("Очередь ходов юнитов перепросчитана");
            }

            public void ClearDeadUnits()
            {
                foreach (var unit in Units)
                {
                    if (unit.State == CombatCore.UnitState.Dead)
                        ReportInfo($"Юнит {unit.Character.Name} умер");
                    if (unit.State == CombatCore.UnitState.Escaped)
                        ReportInfo($"Юнит {unit.Character.Name} сбежал");
                }
                Units = Units
                    .Where(unit => unit.State == CombatCore.UnitState.Fine)
                    .ToList();
                FillQueueWithUnits();
            }
        }
        
        public class UnitComparer : IComparer<CombatCore.PlayerCombatUnit>
        {
            public int Compare(CombatCore.PlayerCombatUnit first, CombatCore.PlayerCombatUnit second)
            {
                if (first.Initiative < second.Initiative)
                    return -1;
                if (first.Initiative > second.Initiative)
                    return 1;
                if (first.Character.Owner != null && second.Character.Owner == null)
                    return -1; 
                if (first.Character.Owner == null && second.Character.Owner != null)
                    return 1;
                return 0;
            }
        }
    }
}