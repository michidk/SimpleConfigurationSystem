namespace SimpleConfigurationSystem
{
    public class LoadedConfigurationResult<T>
    {
        public T Configuration { get; protected internal set; }
        public LoadedConfigurationAction Action { get; protected internal set; }

        public LoadedConfigurationResult() {}

        public LoadedConfigurationResult(T configuration, LoadedConfigurationAction action)
        {
            Configuration = configuration;
            Action = action;
        }
    }
}