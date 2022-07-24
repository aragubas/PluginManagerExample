namespace PluginCore
{
    public struct PluginInitialization
    {
        public bool Successful;
        public string Message;

        public PluginInitialization(bool Successful, string Message = "")
        {
            this.Successful = Successful;
            this.Message = Message;
        }
    }
}