using PluginCore;

namespace HelloPlugin
{
    public class Plugin : IPlugin
    {
        public int Counter;
        public PluginInformation Metadata { get; set; }
        ExecutingContext context;

        public Plugin()
        {
            // Set plugin metadata
            Metadata = new PluginInformation() { Name = "HelloPlugin",
                                                 Description = "Hello World in a Plugin!",
                                                 IsCommandProcessor = false,
                                                 Version = new PluginVersion(1, 0, 1)
            };
        }

        public void Run(object[]? arguments = null)
        {
            Counter++;

            Console.WriteLine($"Hewooo World, the count is: {Counter}");
        }

        public PluginInitialization Initialize(ref ExecutingContext context)
        {
            this.context = context;

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("HelloPlugin initialization");
            Console.ResetColor();

            return new PluginInitialization(true);
        }
    }
}