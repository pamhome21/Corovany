using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.SignalR;
using System.Web;
using Corovany.Logic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Corovany
{
    public class GameController
    {
        private static Dictionary<string, Type> _commandTypes = new Dictionary<string, Type>()
        {
            {"AddPlayerCommand", typeof(AddPlayerCommand)},
            {"InitializeGameCommand", typeof(InitializeGameCommand)},
            {"InitializeCombatSystemCommand", typeof(InitializeCombatSystemCommand)},
            {"NextTurnCommand",typeof(NextTurnCommand)},
            {"InitializeGameStateResetCommand", typeof(InitializeGameStateResetCommand)},
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

        private void HandleData(FrontendCommands.IFrontendCommand data)
        {
            _hub.Clients.All.SendCoreAsync("newCommand", new object[]
            {
                data.GetType().Name,
                JsonConvert.SerializeObject(data.Payload, new JsonSerializerSettings{ReferenceLoopHandling = ReferenceLoopHandling.Serialize})
            });
        }

        private class CommandStore
        {
            public string Type { get; set; }
            public object[] Args { get; set; }
        }
    }
}