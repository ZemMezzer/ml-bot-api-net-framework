using System;
using System.Collections.Generic;

namespace NeuralNetworkBotApiNetFramework.Bots
{
    [Serializable]
    public struct BotConfig
    {
        public string Name;
        public string ApiUrl;
        public string BotToken;
        public string BotId;
        public string InnerMessage;
        public string Context;
        public List<string> StoppingStrings;
        public List<string> NameAliases;
        
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Name))
                return false;

            if (string.IsNullOrEmpty(ApiUrl))
                return false;

            if (string.IsNullOrEmpty(BotToken))
                return false;

            if (string.IsNullOrEmpty(BotId))
                return false;

            return true;
        }
    }
}