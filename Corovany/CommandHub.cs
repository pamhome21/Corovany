using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Corovany
{
    public class CommandHub : Hub
    {
        private GameController _controller;
        public CommandHub(GameController controller)
        {
            _controller = controller;
        }
        public async Task NewCommand(string command)
        {
            _controller.HandleCommand(command);
        }
    }
}