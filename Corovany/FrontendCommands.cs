using System.Collections.Generic;
using Corovany.Logic;

namespace Corovany
{
    namespace FrontendCommands
    {
        public interface IFrontendCommand
        {
            public object Payload { get; }
        }

        public class PlayerAdded : IFrontendCommand
        {
            public object Payload { get; }
            public PlayerAdded(IReadOnlyList<GameCore.Player> payload) => Payload = payload;
        }

        public class GameInitialized : IFrontendCommand
        {
            public object Payload { get; }
            public GameInitialized(IReadOnlyList<CharacterCore.Character> payload) => Payload = payload;
        }

        public class BattleFieldUpdated : IFrontendCommand
        {
            public object Payload { get; }

            public BattleFieldUpdated(CombatCore.PlayerCombatUnit currentUnit,
                IReadOnlyList<CombatCore.PlayerCombatUnit> units,
                Queue<CombatCore.PlayerCombatUnit> queue,
                int turnCounter) =>
                Payload = new BattleFieldPayload
                    {
                        CurrentUnit = currentUnit,
                        Units = units,
                        Queue = new List<CombatCore.PlayerCombatUnit>(queue),
                        TurnCounter = turnCounter
                    };
        }
        
        public class BattleFieldPayload
        {
            public CombatCore.PlayerCombatUnit CurrentUnit;
            public IReadOnlyList<CombatCore.PlayerCombatUnit> Units;
            public IReadOnlyList<CombatCore.PlayerCombatUnit> Queue;
            public int TurnCounter;

        }

        public class BattleEnd : IFrontendCommand
        {
            public object Payload { get; }
            public BattleEnd(bool hasWon) => Payload = hasWon;
        }

        public class Reset : IFrontendCommand
        {
            public object Payload { get; }
            public Reset(bool wasReset) => Payload = wasReset;
        }

        public class FrontendError : IFrontendCommand
        {
            public object Payload { get; }
            public FrontendError(string text) => Payload = text;
        }

        public class FrontendSpellLog : IFrontendCommand
        {
            public object Payload { get; }
            public FrontendSpellLog(CombatCore.PlayerCombatUnit currentUnit, 
                CharacterCore.Perk perk,
                CombatCore.PlayerCombatUnit targetUnit) => 
                Payload = new SpellPayload()
                {
                    CurrentUnit = currentUnit,
                    Perk = perk,
                    Target = targetUnit
                };
        }
        
        public class SpellPayload
        {
            public CombatCore.PlayerCombatUnit CurrentUnit;
            public CharacterCore.Perk Perk;
            public CombatCore.PlayerCombatUnit Target;
        }
    }
}