using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultCommandProcessor.Built_In
{
    internal interface IBuiltInCommand
    {
        public string Description { get; }

        public abstract void Run(string[]? arguments = null);
    }

    static class BuiltInCommands
    {
        public static Dictionary<string, IBuiltInCommand> builtIns = new() 
        {
            { "help", new Help() }
        };
    }
}
