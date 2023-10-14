﻿using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace NeuralNetworkBotApiNetFramework.Handlers
{
    internal static class SaveDataHandler
    {
        private const string SavePath = "SaveData/";
        
        public static void SaveData(string key, object data)
        {
            if (!Directory.Exists(SavePath))
                Directory.CreateDirectory(SavePath);

            string path = $"{SavePath}{key}";
            
            using var fileStream = new FileStream(path, FileMode.OpenOrCreate);
            
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fileStream, data);
        }
        
        public static bool TryLoadData<T>(string key, out T data)
        {
            data = default;
            string path = $"{SavePath}{key}";

            if (!Directory.Exists(SavePath) || !File.Exists(path))
                return false;

            try
            {
                using var fileStream = new FileStream(path, FileMode.OpenOrCreate);
                
                IFormatter formatter = new BinaryFormatter();
                object result = formatter.Deserialize(fileStream);

                if (result is not T dataInstance) 
                    return false;
                    
                data = dataInstance;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}