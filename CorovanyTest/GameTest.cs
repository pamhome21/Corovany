using System.Collections.Generic;
using Corovany.logic;
using NUnit.Framework;

namespace CorovanyTest
{
    public class Tests
    {
        [Test]
        public void AddPlayerTest()
        {
            var testGame = new GameLogicHandler(s =>
            {
                Assert.AreEqual("Игрок 123 с ID 321 создан", s);
            });
            var addPlayer = new AddPlayerCommand("321","123");
            testGame.ExecuteLogicEventCommand(addPlayer);
        }
        
        [Test]
        public void InitGameTest()
        {
            var commandArray = new List<string>();
            var testGame = new GameLogicHandler(s =>
            {
                commandArray.Add(s);
            });
            var addPlayer = new AddPlayerCommand("321","123");
            testGame.ExecuteLogicEventCommand(addPlayer);
            var initializeGame = new InitializeGameCommand();
            testGame.ExecuteLogicEventCommand(initializeGame);
            CollectionAssert.AreEqual(new []
            {
                "Игрок 123 с ID 321 создан",
                "Врубаем экран"
            }, commandArray);
        }

        [Test]
        public void InitCombatSystemTest()
        {
            var commandArray = new List<string>();
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
            CollectionAssert.AreEqual(new []{
                "Игрок 123 с ID 321 создан",
                "Врубаем экран",
                "Очередь ходов юнитов перепросчитана",
                "Ход персонажа Kek игрока 123"}, commandArray);
        }
        
        [Test]
        public void BasicCombatSystemTest()
        {
            var commandArray = new List<string>();
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
            CollectionAssert.AreEqual(new []{
                "Игрок 123 с ID 321 создан",
                "Врубаем экран",
                "Очередь ходов юнитов перепросчитана",
                "Ход персонажа Kek игрока 123",
                "Цель с названием KekBot: применён эффект перка Kek",
                "Очередь ходов юнитов перепросчитана",
                "Ход персонажа SlowKek игрока 123"
            }, commandArray);
        }
        
        [Test]
        public void BasicCombatSystemEndTest()
        {
            var commandArray = new List<string>();
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
            CollectionAssert.AreEqual(new []{
                "Игрок 123 с ID 321 создан",
                "Врубаем экран",
                "Очередь ходов юнитов перепросчитана",
                "Ход персонажа Kek игрока 123",
                "Цель с названием KekBot: применён эффект перка Kek",
                "Очередь ходов юнитов перепросчитана",
                "Ход персонажа SlowKek игрока 123",
                "Цель с названием SlowKekBot: применён эффект перка Kek",
                "Очередь ходов юнитов перепросчитана",
                "Победа"
            }, commandArray);
        }
        
        [Test]
        public void BasicCombatSystemWithFFTest()
        {
            var commandArray = new List<string>();
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
            CollectionAssert.AreEqual(new []{
                "Игрок 123 с ID 321 создан",
                "Врубаем экран",
                "Очередь ходов юнитов перепросчитана",
                "Ход персонажа Kek игрока 123",
                "Цель с названием KekBot: применён эффект перка Kek",
                "Очередь ходов юнитов перепросчитана",
                "Ход персонажа SlowKek игрока 123",
                "Цель с названием Kek: применён эффект перка Kek",
                "Очередь ходов юнитов перепросчитана",
                "Ход персонажа SlowKek игрока 123"
            }, commandArray);
        }
    }
}