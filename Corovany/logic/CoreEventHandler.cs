using System;

namespace Corovany.logic
{
    public class CoreEventHandler
    {
        public class GameLogicHandler
        {
            private GameCore.Game Game { get; }

            public GameLogicHandler(Action<string> reportInfo)
            {
                Game = new GameCore.Game(reportInfo);
            }

            public void ExecuteLogicEventCommand(ICommand command)
            {
                command.ExecuteCommand(Game);
            }
        }
        
        public interface ICommand
        {
            public void ExecuteCommand(GameCore.Game game);
        }
        
        public class AddPlayerCommand : ICommand
        {
            private (string Id, string Name) PlayerData { get; set; }
            public AddPlayerCommand(string id, string name)
            {
                PlayerData = (id, name);
            }
            public void ExecuteCommand(GameCore.Game game)
            {
                game.Players.Add(PlayerData.Id, new GameCore.Player(PlayerData.Name));
                game.Enemies.Add(new GameCore.Enemy());
            }
        }
        
        public class InitializeGameCommand : ICommand
        {
            public void ExecuteCommand(GameCore.Game game)
            {
                foreach (var (_, player) in game.Players)
                {
                    player.CurrentChars[0] = new CharacterCore.Character("Kek", CharClasses.TestificateCl.Class, 1, player);
                    game.Units.Add(new CombatCore.PlayerCombatUnit(player.CurrentChars[0]));
                }

                foreach (var enemy in game.Enemies)
                {
                    enemy.EnemyChars.Add(new CharacterCore.Character("Kek", CharClasses.TestificateCl.Class, 1));
                    game.Units.Add(new CombatCore.PlayerCombatUnit(enemy.EnemyChars[0]));
                }
                game.ReportInfo("Врубаем экран");
            }
        }
        
        public class InitializeCombatSystemCommand : ICommand
        {
            public void ExecuteCommand(GameCore.Game game)
            {
                FillQueueWithUnits(game);
                
            }

            private void FillQueueWithUnits(GameCore.Game game)
            {
                foreach (var unit in game.Units)
                {
                    game.Queue.Enqueue(unit);
                    if (unit.Character.Owner != null)
                        game.PlayerUnitCounter++;
                    else
                        game.EnemyUnitCounter++;
                }
            }

            private void CombatController(GameCore.Game game)
            {
                
            }
        }
    }
}