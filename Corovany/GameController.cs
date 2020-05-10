using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.SignalR;
using System.Web;
using Corovany.logic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Corovany
{
    public class GameController
    {
        private static Dictionary<string, Type> _commandTypes = new Dictionary<string, Type>()
        {
            {"InitializeGameCommand", typeof(InitializeGameCommand)}
        };

        private readonly GameLogicHandler _game;
        private readonly IHubContext<CommandHub> _hub;

        public GameController(IHubContext<CommandHub> hub)
        {
            Console.WriteLine("Started game controller");
            _hub = hub;
            _game = new GameLogicHandler(HandleData);
        }

        public void HandleCommand(string command)
        {
            Console.WriteLine(command);
            try
            {
                var commandStore = JsonConvert.DeserializeObject<CommandStore>(command);
                if (!_commandTypes.ContainsKey(commandStore.Type)) throw new DataException();
                var commandInstance =
                    (ICommand) Activator.CreateInstance(_commandTypes[commandStore.Type], commandStore.Args);
                _game.ExecuteLogicEventCommand(commandInstance);
            }
            catch (JsonException)
            {
                Console.WriteLine("Error in json");
            }
            catch (DataException)
            {
                Console.WriteLine("Wrong command name");
            }
        }

        private void HandleData(string data)
        {
            _hub.Clients.All.SendCoreAsync("newCommand", new object[] {data});
        }

        private class CommandStore
        {
            public string Type { get; set; }
            public object[] Args { get; set; }
        }
    }
}