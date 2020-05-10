using System;
using System.Collections.Generic;

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

            public Game(Action<string> reportInfo)
            {
                Players = new Dictionary<string, Player>();
                Enemies = new List<Enemy>();
                Units = new List<CombatCore.PlayerCombatUnit>();
                UnitTurnQueue = new Queue<CombatCore.PlayerCombatUnit>();
                ReportInfo = reportInfo;
            }
        }
    }
}