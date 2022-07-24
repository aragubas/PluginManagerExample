using PluginCore;

namespace Echo
{
    public class Plugin : IPlugin
    {
        public PluginInformation Metadata { get; }

        public Plugin()
        {
            Metadata = new PluginInformation()
            {
                Name = "Echo",
                Description = "Repeats all strings in arguments",
                Version = new PluginVersion(1, 0, 0)
            };
        }

        public PluginInitialization Initialize(ref ExecutingContext context)
        {
            return new PluginInitialization(true);
        }

        public void Run(object[]? arguments = null)
        {
            string allText = "";

            foreach(object @object in arguments)
            {
                allText += @object + " ";
            }

            Console.WriteLine(allText.Trim());       
        }
    }
}