using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using MLBotApiNetFramework.Bots;

namespace MLBotApiNetFramework.Handlers
{
    public static class BotConfigsHandler
    {
        public static void CreateConfig(string path, BotConfig config)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (!config.IsValid())
            {
                Console.WriteLine("Can't create config because input value is not valid!");
                return;
            }
            
            string filePath = $"{path}/{config.Name}.cfg";
            
            using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
            
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fileStream, config);
        }

        public static BotConfig[] LoadConfigsAtPath(string path)
        {
            List<BotConfig> configs = new();

            foreach (var file in Directory.GetFiles(path))
            {
                string extension = Path.GetExtension(file);
                
                if(!extension.Contains("cfg"))
                    continue;

                var config = LoadConfigInternal(file);
                
                if(!config.IsValid())
                    continue;
                
                configs.Add(config);
            }

            return configs.ToArray();
        }

        private static BotConfig LoadConfigInternal(string configPath)
        {
            if (!File.Exists(configPath))
                return default;

            try
            {
                using var fileStream = new FileStream(configPath, FileMode.OpenOrCreate);
                
                IFormatter formatter = new BinaryFormatter();
                object result = formatter.Deserialize(fileStream);

                if (result is not BotConfig configInstance) 
                    return default;

                return configInstance;

            }
            catch (Exception) { /* ignored */}
            return default;
        }
    }
}