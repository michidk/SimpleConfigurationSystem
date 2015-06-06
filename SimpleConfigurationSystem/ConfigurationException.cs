using System;

namespace SimpleConfigurationSystem
{
    public class ConfigurationException : Exception
    {
        public ConfigurationException() {}

        public ConfigurationException(string message, string fileName) : this(message, fileName, null) {}

        public ConfigurationException(string message, string fileName, Exception inner) : base("Error while deserializing configuration: " + message + " in " + fileName, inner) {}
    }
}