using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Corovany;
using Corovany.FrontendCommands;
using Corovany.logic;
using NUnit.Framework;

namespace CorovanyTest
{
    public class Tests
    {
        [Test]
        public void AddPlayerTest()
        {
            var commandArray = new List<IFrontendCommand>();
            var testGame = new GameLogicHandler(s =>
            {
                commandArray.Add(s);
            });
            var addPlayer = new AddPlayerCommand("321","123");
            testGame.ExecuteLogicEventCommand(addPlayer); 
            var playerList = (List<GameCore.Player>) commandArray[0].Payload;
            Assert.AreEqual(playerList[0].Id, "321");
            Assert.AreEqual(playerList[0].Name, "123");
        }
        
        [Test]
        public void InitGameTest()
        {
            var commandArray = new List<IFrontendCommand>();
            var testGame = new GameLogicHandler(s =>
            {
                commandArray.Add(s);
            });
            var addPlayer = new AddPlayerCommand("321","123");
            testGame.ExecuteLogicEventCommand(addPlayer);
            var initializeGame = new InitializeGameCommand();
            testGame.ExecuteLogicEventCommand(initializeGame);
            var playerList = (List<GameCore.Player>) commandArray[0].Payload;
            var player = playerList[0];
            var charList = (List<CharacterCore.Character>) commandArray[1].Payload;
            Assert.AreEqual(charList[0].Name, "Kek");
            Assert.AreEqual(charList[0].OwnerId, player.Id);
        }

        [Test]
        public void InitCombatSystemTest()
        {
            var commandArray = new List<IFrontendCommand>();
            var testGame = new GameLogicHandler(s =>
            {
                commandArray.Add(s);
            });
            var addPlayer = new AddPlayerCommand("321","123");
            testGame.ExecuteLogicEventCommand(addPlayer);
            var initializeGame = new InitializeGameCommand();
            testGame.ExecuteLogicEventCommand(initializeGame);
            var initializeCombatSystem = new InitializeCombatSystemCommand();
            testGame.ExecuteLogicEventCommand(initializeCombatSystem);
            var playerList = (List<GameCore.Player>) commandArray[0].Payload;
            var player = playerList[0];
            var battlefieldOverwatch = (BattleFieldPayload)commandArray[2].Payload;
            Assert.AreEqual(battlefieldOverwatch.CurrentUnit.Character.Name, "Kek");
            Assert.AreEqual(battlefieldOverwatch.CurrentUnit.Character.OwnerId, player.Id);
        }
        
        [Test]
        public void BasicCombatSystemTest()
        {
            var commandArray = new List<IFrontendCommand>();
            var testGame = new GameLogicHandler(s =>
            {
                commandArray.Add(s);
            });
            var addPlayer = new AddPlayerCommand("321","123");
            testGame.ExecuteLogicEventCommand(addPlayer);
            var initializeGame = new InitializeGameCommand();
            testGame.ExecuteLogicEventCommand(initializeGame);
            var initializeCombatSystem = new InitializeCombatSystemCommand();
            testGame.ExecuteLogicEventCommand(initializeCombatSystem);
            var turn1 = new NextTurnCommand("Kek", "KekBot");
            testGame.ExecuteLogicEventCommand(turn1);
            var battlefieldOverwatch = (BattleFieldPayload)commandArray[3].Payload;
            Assert.AreEqual(battlefieldOverwatch.Units.First(unit => unit.Character.Name=="KekBot").State, 
                CombatCore.UnitState.Dead);
        }
        
        [Test]
        public void BasicCombatSystemEndTest()
        {
            var commandArray = new List<IFrontendCommand>();
            var testGame = new GameLogicHandler(s =>
            {
                commandArray.Add(s);
            });
            var addPlayer = new AddPlayerCommand("321","123");
            testGame.ExecuteLogicEventCommand(addPlayer);
            var initializeGame = new InitializeGameCommand();
            testGame.ExecuteLogicEventCommand(initializeGame);
            var initializeCombatSystem = new InitializeCombatSystemCommand();
            testGame.ExecuteLogicEventCommand(initializeCombatSystem);
            var turn1 = new NextTurnCommand("Kek", "KekBot");
            testGame.ExecuteLogicEventCommand(turn1);
            var turn2 = new NextTurnCommand("Kek", "SlowKekBot");
            testGame.ExecuteLogicEventCommand(turn2);
            var battlefieldOverwatch = (BattleFieldPayload)commandArray[4].Payload;
            var hasWon = (bool) commandArray[5].Payload;
            Assert.AreEqual(battlefieldOverwatch.Queue.Count(unit => unit.Character.OwnerId==null), 0);
            Assert.AreEqual(hasWon, true);
        }
        
        [Test]
        public void BasicCombatSystemWithFFTest()
        {
            var commandArray = new List<IFrontendCommand>();
            var testGame = new GameLogicHandler(s =>
            {
                commandArray.Add(s);
            });
            var addPlayer = new AddPlayerCommand("321","123");
            testGame.ExecuteLogicEventCommand(addPlayer);
            var initializeGame = new InitializeGameCommand();
            testGame.ExecuteLogicEventCommand(initializeGame);
            var initializeCombatSystem = new InitializeCombatSystemCommand();
            testGame.ExecuteLogicEventCommand(initializeCombatSystem);
            var turn1 = new NextTurnCommand("Kek", "KekBot");
            testGame.ExecuteLogicEventCommand(turn1);
            var turn2 = new NextTurnCommand("Kek", "Kek");
            testGame.ExecuteLogicEventCommand(turn2);
            var battlefieldOverwatch = (BattleFieldPayload)commandArray[4].Payload;
            Assert.AreEqual(battlefieldOverwatch.Queue.Count(unit => unit.Character.OwnerId!=null), 1);
        }

        [Test]
        public void InitializeGameStateResetTest()
        {
            var commandArray = new List<IFrontendCommand>();
            var testGame = new GameLogicHandler(s =>
            {
                commandArray.Add(s);
            });
            var addPlayer = new AddPlayerCommand("321","123");
            testGame.ExecuteLogicEventCommand(addPlayer);
            var initializeGame = new InitializeGameCommand();
            testGame.ExecuteLogicEventCommand(initializeGame);
            var initializeCombatSystem = new InitializeCombatSystemCommand();
            testGame.ExecuteLogicEventCommand(initializeCombatSystem);
            var turn1 = new NextTurnCommand("Kek", "KekBot");
            testGame.ExecuteLogicEventCommand(turn1);
            var reset = new InitializeGameStateResetCommand();
            testGame.ExecuteLogicEventCommand(reset);
            var addPlayerAfterReset = new AddPlayerCommand("123","321");
            testGame.ExecuteLogicEventCommand(addPlayerAfterReset);
            var playerList = (List<GameCore.Player>) commandArray[5].Payload;
            var player = playerList[0];
            Assert.AreEqual(player.Id, "123");
            Assert.AreEqual(player.Name, "321");
            Assert.AreEqual(playerList.Count, 1);
        }
    }
}