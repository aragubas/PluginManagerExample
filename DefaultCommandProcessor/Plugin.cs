using DefaultCommandProcessor.Built_In;
using PluginCore;

namespace DefaultCommandProcessor
{
    public class Plugin : IPlugin
    {
        public PluginInformation Metadata { get; }
        ExecutingContext context;

        public Plugin()
        {
            // Set plugin metadata
            Metadata = new PluginInformation()
            {
                Name = "DefaultCommandProcessor",
                Description = "the thing that process what you type and do stuff",
                IsCommandProcessor = true, 
                Version = new PluginVersion(1, 0, 0)
            };
        }

        public PluginInitialization Initialize(ref ExecutingContext context)
        {
            this.context = context;
            return new PluginInitialization(true);
        }

        void PrintWelcome()
        {
            Console.WriteLine($"\n\nWelcome to DefaultCommandProcessor v{Metadata.Version}");
            Console.WriteLine("Type 'help' to see list of built-in commands\n\n");
        }

        void InvalidCommand(string command)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"'{command}': built-in command or plugin not found.");
            Console.ResetColor();
            Console.Write("\n");
        }

        public void Run(string[]? arguments = null)
        {
            bool IsRunning = true;
            string Username = Environment.UserName;
            
            // Print welcome message
            PrintWelcome();

            while (IsRunning)
            {
                Console.ResetColor();
                Console.Write("$: ");
                string Input = Console.ReadLine().Trim();

                string[] SplitInput = Input.Split(' ');

                // Tries to find a matching built-in command
                if (BuiltInCommands.builtIns.TryGetValue(SplitInput[0], out IBuiltInCommand command))
                {
                    command.Run(SplitInput);

                }
                else
                {
                    InvalidCommand(SplitInput[0]);
                    continue;
                }


            }
        }

        public void SayHello() { }
    }
}