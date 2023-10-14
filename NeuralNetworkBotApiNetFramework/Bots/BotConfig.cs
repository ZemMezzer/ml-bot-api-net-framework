using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetworkBotApiNetFramework.Bots
{
    [Serializable]
    public struct BotConfig
    {
        private string _name;
        private string _apiUrl;
        private string _botToken;
        private string _botId;
        private string _innerMessage;
        private List<string> _nameAliases;

        public string Name => _name;
        public string ApiUrl => _apiUrl;
        public string BotToken => _botToken;
        public string BotId => _botId;
        public string InnerMessage => _innerMessage;
        public IReadOnlyList<string> NameAliases => _nameAliases;

        public BotConfig(string name, string apiUrl, string botToken, string botId, string innerMessage, IReadOnlyList<string> nameAliases)
        {
            _name = name;
            _apiUrl = apiUrl;
            _botToken = botToken;
            _botId = botId;
            _innerMessage = innerMessage;
            _nameAliases = nameAliases.ToList();
        }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(_name))
                return false;

            if (string.IsNullOrEmpty(_apiUrl))
                return false;

            if (string.IsNullOrEmpty(_botToken))
                return false;

            if (string.IsNullOrEmpty(_botId))
                return false;

            return true;
        }
    }
}