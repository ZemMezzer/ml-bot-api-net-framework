using System;
using System.Collections.Generic;
using ChatBotsApi.Core;
using ChatBotsApi.Core.Messages.Data;
using NeuralNetworkBotApiNetFramework.Api.TextGeneration;
using NeuralNetworkBotApiNetFramework.Api.TextGeneration.Data;
using NeuralNetworkBotApiNetFramework.Handlers;
using NeuralNetworkBotApiNetFramework.Providers;

namespace NeuralNetworkBotApiNetFramework.Bots
{
    public abstract class ChatBot
    {
        public Bot Bot { get;}
        private Dictionary<UserData, History> _userHistories = new();
        public BotConfig Config { get; }

        private readonly TextGenerationRequestQueue _requestQueue;
        private readonly InputConverterProvider _convertProvider;
        private readonly CommandsProvider _commandsProvider;

        public ChatBot(Bot bot, BotConfig config, TextGenerationRequestQueue requestQueue, CommandsProvider commandsProvider)
        {
            Config = config;
            Bot = bot;
            _requestQueue = requestQueue;
            _convertProvider = new InputConverterProvider();
            _commandsProvider = commandsProvider;
        }
        
        public void StartMessageReceiving()
        {
            Bot.OnMessageReceived += OnMessageReceived;
            Bot.OnBotMessageForwarded += OnBotMessageForwarded;
        }

        private void OnBotMessageForwarded(MessageData currentMessage, MessageData forwardedMessage)
        {
            Console.WriteLine($"Received message from: {currentMessage.Sender.UserName}");
            SendMessageRequest(currentMessage, currentMessage.Message);
        }

        private void OnMessageReceived(MessageData message)
        {
            Console.WriteLine($"Received message from: {message.Sender.UserName}");
            if(_commandsProvider.TryExecuteCommand(message, this))
                return;
            
            string input = _convertProvider.Convert(message, Config);
            SendMessageRequest(message, input);
        }

        private void SendMessageRequest(MessageData sourceMessage, string formattedResult)
        {
            var request = new ApiRequest(sourceMessage.Sender, sourceMessage.ChatData, formattedResult, this, OnRequestComplete);
            _requestQueue.AddRequestInQueue(request);
        }

        private void OnRequestComplete(string result, ChatData chat)
        {
            OnMessageGenerationComplete(result, chat);
        }

        protected abstract void OnMessageGenerationComplete(string botMessage, ChatData chat);

        public History GetUserHistory(UserData user)
        {
            if (!_userHistories.ContainsKey(user))
                _userHistories.Add(user, new History(Config.InnerMessage));
            
            return _userHistories[user];
        }
        
        public void LoadData()
        {
            if (SaveDataHandler.TryLoadData($"{Bot.Name}.bin", out Dictionary<UserData, History> histories))
                _userHistories = histories;
        }

        public void SaveData()
        {
            SaveDataHandler.SaveData($"{Bot.Name}.bin", _userHistories);
        }
    }
}