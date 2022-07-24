using DefaultCommandProcessor.Built_In;
using PluginCore;

namespace DefaultCommandProcessor
{
    public class Plugin : IPlugin
    {
        public PluginInformation Metadata { get; }
        public ExecutingContext context;

        public Plugin()
        {
            // Set plugin metadata
            Metadata = new PluginInformation()
            {
                Name = "DefaultCommandProcessor",
                Description = "Default command processor!",
                IsCommandProcessor = true, 
                Version = new PluginVersion(1, 0, 1)
            };
        }

        public PluginInitialization Initialize(ref ExecutingContext context)
        {
            this.context = context;
            BuiltInCommands.context = context;
            return new PluginInitialization(true);
        }

        void PrintWelcome()
        {
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            
            Console.Write($"\n");
            Console.Write($"┌┤ Welcome to DefaultCommandProcessor v{Metadata.Version} ├┐\n");
            Console.Write($"└┤ Type 'help' to see a list of commands     ├┘");

            Console.ResetColor();
            Console.Write($"\n\n");
        }

        void InvalidCommand(string command)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"─┤ '{command}': built-in command or plugin not found.");
            Console.ResetColor();
            Console.Write("\n");
        }

        void FillLine(int Y)
        {
            Console.SetCursorPosition(0, Y);
            for (int i = 0; i < Console.BufferWidth - 1; i++)
            {
                Console.Write(' ');
                Console.SetCursorPosition((i + 1), Y);
            }
        }

        public void Run(object[]? arguments = null)
        {
            bool IsRunning = true;
            string Username = Environment.UserName;
            IPlugin? newCommandProcessorRef = null;
            string[]? newCommandProcessorArguments = null;

            // Print welcome message
            PrintWelcome();

            while (IsRunning)
            {
                // Move the cursor back to the begging of the last line and write the prompt indicator
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("$:");
                Console.ResetColor();
                Console.Write(" ");
                 
                string? Input = Console.ReadLine();
                if (Input == null) { continue; }
                Input = Input.Trim();

                string[] SplitInput = Input.Split(' ');
                
                if (SplitInput[0].All(character => char.IsWhiteSpace(character)))
                {
                    continue;
                }

                // Flow-Control command 
                if (SplitInput[0] == "exit")
                {
                    IsRunning = false;
                    break;
                }

                // Tries to find a matching built-in command
                if (BuiltInCommands.builtIns.TryGetValue(SplitInput[0], out IBuiltInCommand command))
                {
                    command.Run(SplitInput);
                    continue;
                }

                IPlugin? plugin = context.Plugins.Find(plugin => plugin.Metadata.Name == SplitInput[0]);

                // Invalid command!
                if (plugin == null)
                {
                    InvalidCommand(SplitInput[0]);
                    continue;
                }

                string[] args = new string[Math.Max(1, SplitInput.Length - 1)];
                for(int i = 1; i < SplitInput.Length; i++)
                {
                    args[i - 1] = SplitInput[i];
                }

                // If the calling plugin is another command processor
                if (plugin.Metadata.IsCommandProcessor) 
                {
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("─┤ Exiting DefaultCommandProcessor...");
                    Console.ResetColor();
                    Console.Write("\n");

                    newCommandProcessorRef = plugin;
                    newCommandProcessorArguments = args;
                    IsRunning = false;
                    break;
                }

                plugin.Run(args);
            }

            // Change to new command processor
            if (newCommandProcessorRef != null)
            {
                newCommandProcessorRef.Run(newCommandProcessorArguments);
            }
        }
    }
}