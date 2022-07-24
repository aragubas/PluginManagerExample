using System.Text.Json;

namespace PluginCore
{
    public struct PluginInformation
    {
        public string Name;
        public string Description;
        public bool IsCommandProcessor = false;
        public PluginVersion Version;

        public PluginInformation(string name, string description, bool isCommandProcessor, PluginVersion version)
        {
            Name = name;
            Description = description;
            IsCommandProcessor = isCommandProcessor;
            Version = version;
        }

        public override string ToString()
        {
            return $"Plugin {Name}";
        }
    }
}