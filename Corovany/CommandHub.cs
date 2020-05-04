using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Corovany
{
    public class CommandHub : Hub
    {
        public async Task NewCommand(string command)
        {
            Console.WriteLine(command);
        }
    }
}