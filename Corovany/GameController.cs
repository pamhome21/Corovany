using System;
using Microsoft.AspNetCore.SignalR;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace Corovany
{
    public class GameController
    {
        
        public GameController(IHubContext<CommandHub> hub)
        {
            
        }
        
        public void HandleCommand(string command)
        {
            
        }
    }
}