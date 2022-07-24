using PluginCore;

namespace DefaultCommandProcessor.Built_In
{
    internal class Help : IBuiltInCommand
    {
        public string Description => "Displays a list of available commands.";

        public void Run(string[]? arguments = null)
        {
            Console.WriteLine("Built-in commands:");
            foreach (KeyValuePair<string, IBuiltInCommand> command in BuiltInCommands.builtIns)
            {
                Console.WriteLine($"{command.Key}: {command.Value.Description}");
            }

            //Console.WriteLine("Plugins:");
            //foreach (KeyValuePair<string, IBuiltInCommand> command in )
            //{
            //    Console.WriteLine($"{command.Key}: {command.Value.Description}");
            //}

        }
    }
}
