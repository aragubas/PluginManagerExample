namespace PluginCore
{
    public interface IPlugin
    {
        public PluginInformation Metadata { get; }

        public abstract void SayHello();
        public abstract void Run(string[]? arguments = null);

        public abstract PluginInitialization Initialize(ref ExecutingContext context);
    }   
}