using System;
using System.IO;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization;

namespace SimpleConfigurationSystem
{
    public abstract class Configuration<T> where T : Configuration<T>
    {
        [YamlIgnore]
        public string FilePath { get; }

        [YamlIgnore]
        public string Name { get; }

        public abstract void LoadDefaults();

        protected Configuration(string filePath)
        {
            this.FilePath = filePath;

            try
            {
                Name = Path.GetFileName(filePath);
            }
            catch (Exception e)
            {
                throw new ConfigurationException("Couldn't acces file", FilePath, e);
            }
            
        }

        public void Save()
        {
            var serizalizer = new Serializer();
            try
            {
                using (TextWriter writer = File.CreateText(FilePath))
                {
                    serizalizer.Serialize(writer, this);
                }
            }
            catch (Exception e)
            {
                throw new ConfigurationException("Couldn't write file", Name, e);
            }
        }

        public void Load()
        {
            var deserializer = new Deserializer();
            string input = "";
            T cfg;
            try
            {
                input = File.ReadAllText(FilePath);
            }
            catch (Exception e)
            {
                throw new ConfigurationException("Couldn't read file", Name, e);
            }

            try
            {
                cfg = deserializer.Deserialize<T>(new StringReader(input));
            }
            catch (Exception e)
            {
                throw new ConfigurationException("Couldn't deserialize file", Name, e);
            }

            if (String.IsNullOrEmpty(input) || cfg == null)
            {
                throw new ConfigurationException("Configuration is Null", Name);
            }

            foreach (var prop in typeof(T).GetProperties())
            {
                var val = prop.GetValue(cfg);
                if (val == null)
                {
                    throw new ConfigurationException("Value of property " + prop.Name + " is Null", Name);
                }
                prop.SetValue(this, val);
            }
        }

        public static LoadedConfigurationResult<T> LoadConfig<T>(string filePath, bool logToConsole = false) where T : Configuration<T>
        {
            var result = new LoadedConfigurationResult<T>();
            T cfg = Activator.CreateInstance<T>();

            if (!cfg.Exists())
            {
                LogToConsole(filePath + " doesn't exist. Creating default one. Enter your login data and restart.", logToConsole);

                cfg.LoadDefaults();
                cfg.Save();

                LogToConsole("Default " + cfg.Name + " created.", logToConsole);

                result.Action = LoadedConfigurationAction.CreatedDefault;
            }
            else
            {
                LogToConsole("Loading " + cfg.Name + "...", logToConsole);

                cfg.Load();
            }

            result.Configuration = cfg;
            return result;
        }

        public bool Exists()
        {
            bool success = false;
            try
            {
                success = File.Exists(FilePath) && new FileInfo(FilePath).Length > 0;
            }
            catch (Exception e)
            {
                throw new ConfigurationException("Coudn't access file", Name, e);
            }
            return success;
        }

        public void Delete()
        {
            try
            {
                if (!File.Exists(FilePath))
                    return;

                File.Delete(FilePath);
            }
            catch (Exception e)
            {
                throw new ConfigurationException("Coudn't access and delete file", Name, e);
            }
        }

        public static void LogToConsole(string msg, bool log)
        {
            if (log) Console.WriteLine(msg);
        }
    }
}