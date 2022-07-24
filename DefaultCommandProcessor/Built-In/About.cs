using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultCommandProcessor.Built_In
{
    internal class About : IBuiltInCommand
    {
        public string Description => "Who made this?";

        public void Run(string[]? arguments = null)
        {
            Console.Write("DefaultCommandProcessor by ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Aragubas\n");
            Console.ResetColor();
            Console.Write("Made for PluginManagerExample project.");

            Console.Write("\n");
        }
    }
}
