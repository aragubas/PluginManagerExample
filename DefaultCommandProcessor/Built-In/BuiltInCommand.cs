using PluginCore;

namespace DefaultCommandProcessor.Built_In
{
    internal interface IBuiltInCommand
    {
        public string Description { get; }

        public abstract void Run(string[]? arguments = null);
    }

    static class BuiltInCommands
    {
        public static ExecutingContext context;

        public static Dictionary<string, IBuiltInCommand> builtIns = new()
        {
            { "help", new Help() },
            { "clear", new Clear() },
            { "about", new About() }
        };

    }
}
