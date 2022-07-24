using PluginCore;

namespace DefaultCommandProcessor.Built_In
{
    internal class Help : IBuiltInCommand
    {
        public string Description => "Displays a list of available commands.";

        public void Run(string[]? arguments = null)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Format: <name>: <description>\n");
            Console.ResetColor();

            Console.WriteLine("Flow-control commands:");
            Console.WriteLine("  exit: exits DefaultCommandProcessor\n");

            Console.WriteLine("Built-in commands:");
            foreach (KeyValuePair<string, IBuiltInCommand> command in BuiltInCommands.builtIns)
            {
                Console.WriteLine($"  {command.Key}: {command.Value.Description}");
            }

            Console.WriteLine("\nPlugins:");
            Console.WriteLine("Command processors are marked in blue");
            foreach (IPlugin plugin in BuiltInCommands.context.Plugins)
            {
                if (plugin.Metadata.IsCommandProcessor)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.Write($"  {plugin.Metadata.Name}: {plugin.Metadata.Description}");
                if (plugin.Metadata.IsCommandProcessor) { Console.ResetColor(); }
                Console.Write("\n");
            }

            Console.WriteLine("");
        }
    }
}
