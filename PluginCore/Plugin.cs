namespace PluginCore
{
    public interface IPlugin
    {
        public PluginInformation Metadata { get; }

        public abstract void Run(object[]? arguments = null);

        public abstract PluginInitialization Initialize(ref ExecutingContext context);
    }   
}