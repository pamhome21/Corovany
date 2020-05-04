using System;
using Microsoft.AspNetCore.SignalR;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace Corovany
{
    public class GameController
    {
        public static GameController Current;
        public GameController(IHubContext<CommandHub> hub)
        {
            Console.WriteLine("Command controller created");
        }
        
        public void HandleCommand(string command)
        {
            
        }
    }
}