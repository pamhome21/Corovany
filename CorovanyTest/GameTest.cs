using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Corovany;
using Corovany.FrontendCommands;
using Corovany.Logic;
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
        public void AddPlayerAlreadyExistsTest()
        {
            var commandArray = new List<IFrontendCommand>();
            var testGame = new GameLogicHandler(s =>
            {
                commandArray.Add(s);
            });
            var addPlayer = new AddPlayerCommand("321","123");
            testGame.ExecuteLogicEventCommand(addPlayer);
            var addPlayer2 = new AddPlayerCommand("321","321");
            testGame.ExecuteLogicEventCommand(addPlayer);
            Assert.AreEqual((string)commandArray[1].Payload, "Игрок с ID 321 уже существует");
            var playerList = (List<GameCore.Player>) commandArray[2].Payload;
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
            Assert.AreEqual(battlefieldOverwatch.CurrentUnit.Character.Name, "Sherguhseiuhg");
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
            var turn1 = new NextTurnCommand("Shoot Arrows", "Biba");
            testGame.ExecuteLogicEventCommand(turn1);
            var spellInfo = (SpellPayload) commandArray[3].Payload;
            var battlefieldOverwatch = (BattleFieldPayload)commandArray[4].Payload;
            Assert.AreEqual(spellInfo.CurrentUnit.Character.Name, "Sherguhseiuhg");
            Assert.AreEqual(battlefieldOverwatch.Units.First(unit => unit.Character.Name=="Biba").HealthPoints, 
                CharClasses.Hobbit.Class.HealthPoints-15);
        }
        
        [Test]
        public void BasicCombatSystemDeathTest()
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
            var turn1 = new NextTurnCommand("Shoot Arrows", "Biba");
            testGame.ExecuteLogicEventCommand(turn1);
            var turn2 = new NextTurnCommand("Power Blade", "Biba");
            testGame.ExecuteLogicEventCommand(turn2);
            var battlefieldOverwatch = (BattleFieldPayload)commandArray[6].Payload;
            Assert.AreEqual(battlefieldOverwatch.Queue.Count(unit => unit.Character.OwnerId==null), 2);
        }
        
        [Test]
        public void BasicCombatSystemLosingConditionTest()
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
            var turn = new NextTurnCommand("Nature's Wrath", "Sherguhseiuhg");
            testGame.ExecuteLogicEventCommand(turn);
            turn = new NextTurnCommand("Fire Blade", "Sherguhseiuhg");
            testGame.ExecuteLogicEventCommand(turn);
            for (var i = 0; i < 3; i++)
            {
                turn = new NextTurnCommand("Taunt", "Tlatelolko");
                testGame.ExecuteLogicEventCommand(turn);
            }
            turn = new NextTurnCommand("Shout", "Tlatelolko");
            testGame.ExecuteLogicEventCommand(turn);
            turn = new NextTurnCommand("Fire Blade", "Tlatelolko");
            testGame.ExecuteLogicEventCommand(turn);
            for (var i = 0; i < 3; i++)
            {
                turn = new NextTurnCommand("Taunt", "Kek");
                testGame.ExecuteLogicEventCommand(turn);
            }
            turn = new NextTurnCommand("Power Blade", "Kek");
            testGame.ExecuteLogicEventCommand(turn);
            for (var i = 0; i < 3; i++)
            {
                turn = new NextTurnCommand("Taunt", "Kek");
                testGame.ExecuteLogicEventCommand(turn);
            }
            turn = new NextTurnCommand("Fire Blade", "Kek");
            testGame.ExecuteLogicEventCommand(turn);
            for (var i = 0; i < 3; i++)
            {
                turn = new NextTurnCommand("Taunt", "Kek");
                testGame.ExecuteLogicEventCommand(turn);
            }
            turn = new NextTurnCommand("Fire Blade", "Kek");
            testGame.ExecuteLogicEventCommand(turn);
            var hasWon = (bool)commandArray.Last().Payload;
            Assert.AreEqual(hasWon, false);
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
            var turn1 = new NextTurnCommand("Shoot Arrows", "Shrek");
            testGame.ExecuteLogicEventCommand(turn1);
            var reset = new InitializeGameStateResetCommand();
            testGame.ExecuteLogicEventCommand(reset);
            Assert.AreEqual((bool)commandArray[5].Payload, true);
            var addPlayerAfterReset = new AddPlayerCommand("123","321");
            testGame.ExecuteLogicEventCommand(addPlayerAfterReset);
            var playerList = (List<GameCore.Player>) commandArray[6].Payload;
            var player = playerList[0];
            Assert.AreEqual(player.Id, "123");
            Assert.AreEqual(player.Name, "321");
            Assert.AreEqual(playerList.Count, 1);
        }

        [Test]
        public void GetDataTest()
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
            var receiveFullDataState = new ReceiveFullDataStateCommand();
            testGame.ExecuteLogicEventCommand(receiveFullDataState);
            Assert.AreEqual(commandArray[0].Payload, commandArray[3].Payload);
            Assert.AreEqual(commandArray[1].Payload, commandArray[4].Payload);
            var bfp1 = (BattleFieldPayload) commandArray[2].Payload;
            var bfp2 = (BattleFieldPayload) commandArray[5].Payload;
            Assert.AreEqual(bfp1.Queue, bfp2.Queue);
        }

        [Test]
        public void CanHealUnitsTest()
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
            var turn1 = new NextTurnCommand("Shoot Arrows", "Kek");
            testGame.ExecuteLogicEventCommand(turn1);
            var turn2 = new NextTurnCommand("Power Blade", "Biba");
            testGame.ExecuteLogicEventCommand(turn2);
            var turn3 = new NextTurnCommand("Taunt", "Sherguhseiuhg");
            testGame.ExecuteLogicEventCommand(turn3);
            var turn4 = new NextTurnCommand("Healing Spell", "Kek");
            testGame.ExecuteLogicEventCommand(turn4);
            var bfp1 = (BattleFieldPayload) commandArray[10].Payload;
            var unitState1 = bfp1.Units.First(unit => unit.Character.Name == "Kek");
            Assert.AreEqual(unitState1.HealthPoints, unitState1.Character.HealthPoints);
        }
    }
}