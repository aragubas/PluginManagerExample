using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultCommandProcessor.Built_In
{
    internal class Clear : IBuiltInCommand
    {
        public string Description => "Clears the screen";

        public void Run(string[]? arguments = null) => Console.Clear();
    }
}
