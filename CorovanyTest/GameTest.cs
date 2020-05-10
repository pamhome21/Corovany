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
            CollectionAssert.AreEqual(new []{"Игрок 123 с ID 321 создан","Врубаем экран"}, commandArray);
        }

        [Test]
        public void InitCombatSystem()
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
            CollectionAssert.AreEqual(new []{"Игрок 123 с ID 321 создан","Врубаем экран","Очередь ходов юнитов заполнена"}, commandArray);
        }
    }
}