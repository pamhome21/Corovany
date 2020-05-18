using System;
using System.Collections.Generic;
using System.Linq;
using Corovany.FrontendCommands;


namespace Corovany.Logic
{
    /// <summary>
    ///      Предоставляет делегат для общения с фронтэндом
    ///      Содержит в экземпляр экранированной от фронтэнда игры
    ///      Обладает методом выполнения команд, который применет их к игре
    /// </summary>
    public class GameLogicHandler
    {
        private GameCore.Game Game { get; }

        public GameLogicHandler(Action<IFrontendCommand> reportInfo)
        {
            Game = new GameCore.Game(reportInfo);
        }

        public void ExecuteLogicEventCommand(ICommand command)
        {
            command.ExecuteCommand(Game);
        }
    }
    
    /// <summary>
    ///      Интерфес команды
    ///      Содержит метод исполнения команды
    /// </summary>
    public interface ICommand
    {
        public void ExecuteCommand(GameCore.Game game);
    }

    /// <summary>
    ///      Команда перезапуска игры
    /// </summary>
    public class InitializeGameStateResetCommand : ICommand
    {
        public void ExecuteCommand(GameCore.Game game)
        {
            game.Reset();
            game.ReportInfo(new Reset(true));
        }
    }

    /// <summary>
    ///      Команда запроса полного состояния игры
    ///      Уведомляет фронтэнд о текущем состоянии игры
    /// </summary>
    public class ReceiveFullDataStateCommand : ICommand
    {
        public void ExecuteCommand(GameCore.Game game)
        {
            game.ReportInfo(new PlayerAdded(game.Players.Values.ToList()));
            if (game.Players.Count!=0)
                game.ReportInfo(new GameInitialized(game.Players
                .SelectMany(player => player.Value.CurrentChars)
                .ToList()));
            if (game.CurrentUnit == null) return;
            game.ReportInfo(new BattleFieldUpdated(game.CurrentUnit, game.Units, game.UnitTurnQueue, game.TurnCounter));
            if (NextTurnCommand.IsPlayerDead(game.Units))
                game.ReportInfo(new BattleEnd(false));
            if (NextTurnCommand.IsEnemyDead(game.Units))
                game.ReportInfo(new BattleEnd(true));
        }
    }

    /// <summary>
    ///      Команда добавления игрока
    ///      Позволяет фронтэнду добавить нового игрока
    /// </summary>
    public class AddPlayerCommand : ICommand
    {
        private (string Id, string Name) PlayerData { get; set; }

        public AddPlayerCommand(string id, string name)
        {
            PlayerData = (id, name);
        }

        public void ExecuteCommand(GameCore.Game game)
        {
            if (game.Players.Keys.Contains(PlayerData.Id))
            {
                game.ReportInfo(new FrontendError($"Игрок с ID {PlayerData.Id} уже существует"));
            }
            else
            {
                game.Players.Add(PlayerData.Id, new GameCore.Player(PlayerData.Id, PlayerData.Name));
                game.Enemies.Add(new GameCore.Enemy());
            }
            game.ReportInfo(new PlayerAdded(game.Players.Values.ToList()));
        }
    }

    /// <summary>
    ///      Команда инициализации игры
    ///      Инициализирует всех игроков и создаёт ботов
    /// </summary>
    public class InitializeGameCommand : ICommand
    {
        public void ExecuteCommand(GameCore.Game game)
        {
            foreach (var (_, player) in game.Players)
            {
                if (player.CurrentChars[0] != null) return;
                player.CurrentChars[0] = new CharacterCore.Character("Kek", CharClasses.Human.Class, 1, player);
                player.CurrentChars[1] =
                    new CharacterCore.Character("Tlatelolko", CharClasses.Magician.Class, 1, player);
                player.CurrentChars[2] =
                    new CharacterCore.Character("Sherguhseiuhg", CharClasses.Elf.Class, 1, player);
                game.Units.Add(new CombatCore.PlayerCombatUnit(player.CurrentChars[0]));
                game.Units.Add(new CombatCore.PlayerCombatUnit(player.CurrentChars[1]));
                game.Units.Add(new CombatCore.PlayerCombatUnit(player.CurrentChars[2]));
            }

            foreach (var enemy in game.Enemies)
            {
                enemy.EnemyChars.Add(new CharacterCore.Character("Biba", CharClasses.Hobbit.Class, 1));
                enemy.EnemyChars.Add(new CharacterCore.Character("Shrek", CharClasses.Ciclopus.Class, 1));
                enemy.EnemyChars.Add(new CharacterCore.Character("Boba", CharClasses.Hobbit.Class, 1));
                game.Units.Add(new CombatCore.PlayerCombatUnit(enemy.EnemyChars[0]));
                game.Units.Add(new CombatCore.PlayerCombatUnit(enemy.EnemyChars[1]));
                game.Units.Add(new CombatCore.PlayerCombatUnit(enemy.EnemyChars[2]));
            }

            if (game.Players.Count!=0)
                game.ReportInfo(new GameInitialized(game.Players
                    .SelectMany(player => player.Value.CurrentChars)
                    .ToList()));
            else
                game.ReportInfo(new FrontendError("Игроки отсутствуют"));
        }
    }

    /// <summary>
    ///      Команда инициализации боя
    ///      Создаёт очередь юнитов, делает текущим юнитом первого юнита из очереди
    ///      Сообщает фронтэнду информацию о юнитах на поле боя
    /// </summary>
    public class InitializeCombatSystemCommand : ICommand
    {
        public void ExecuteCommand(GameCore.Game game)
        {
            game.FillQueueWithUnits();
            game.GetUnitFromQueue();
            game.ReportInfo(new BattleFieldUpdated(game.CurrentUnit, game.Units, game.UnitTurnQueue, game.TurnCounter));
        }
    }

    /// <summary>
    ///      Команда следующего хода
    ///      Применяет переданную ей способность к переданной цели
    ///      Осуществляет ротацию очереди и её перепросчёт
    ///      Сообщает фронтэнду информацию о юнитах на поле боя
    /// </summary>
    public class NextTurnCommand : ICommand
    {
        private string PerkKey { get; set; }
        private string TargetKey { get; set; }

        public NextTurnCommand(string perkKey, string targetKey)
        {
            PerkKey = perkKey;
            TargetKey = targetKey;
        }

        public void ExecuteCommand(GameCore.Game game)
        {
            if (!game.AvailablePerks.ContainsKey(PerkKey))
            {
                game.ReportInfo(new FrontendError($"Перк с названием {PerkKey} не существует"));
                return;
            }

            if (!game.AvailableTargets.ContainsKey(TargetKey))
            {
                game.ReportInfo(new FrontendError($"Цель с названием {TargetKey} не существует"));
                return;
            }

            if (game.CurrentUnit.CanCastPerk(game.AvailablePerks[PerkKey]))
            {
                game.CurrentUnit.CastPerk(game.AvailablePerks[PerkKey], game.AvailableTargets[TargetKey]);
                game.CurrentUnit.DecreaseCooldown();
            }
            else
            {
                game.ReportInfo(new FrontendError($"Невозможно применить способность {PerkKey}"));
                return;
            }
            game.ReportInfo(new FrontendSpellLog(game.CurrentUnit, 
                game.AvailablePerks[PerkKey], game.AvailableTargets[TargetKey]));
            if (game.CurrentUnit.Character.OwnerId == null)
                game.RandomUnitSelect();
            game.FillQueueWithUnits();
            if (IsPlayerDead(game.Units))
            {
                game.ReportInfo(new BattleFieldUpdated(game.CurrentUnit, game.Units, game.UnitTurnQueue, game.TurnCounter));
                game.ReportInfo(new BattleEnd(false));
                return;
            }

            if (IsEnemyDead(game.Units))
            {
                game.ReportInfo(new BattleFieldUpdated(game.CurrentUnit, game.Units, game.UnitTurnQueue, game.TurnCounter));
                game.ReportInfo(new BattleEnd(true));
                return;
            }
            
            game.GetUnitFromQueue();
            game.ReportInfo(new BattleFieldUpdated(game.CurrentUnit, game.Units, game.UnitTurnQueue, game.TurnCounter));
        }

        public static bool IsPlayerDead(List<CombatCore.PlayerCombatUnit> units)
        {
            return units.Count(unit => unit.Character.OwnerId != null
                                       && unit.State == CombatCore.UnitState.Fine) == 0;
        }

        public static bool IsEnemyDead(List<CombatCore.PlayerCombatUnit> units)
        {
            return units.Count(unit => unit.Character.OwnerId == null
                                       && unit.State == CombatCore.UnitState.Fine) == 0;
        }
    }
}