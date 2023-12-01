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
        public BotConfig Config { get; private set; }
        public BotData Data { get; private set; }

        private readonly TextGenerationRequestQueue _requestQueue;
        private readonly InputConverterProvider _convertProvider;
        private readonly CommandsProvider _commandsProvider;

        private bool _isReceiving;

        public ChatBot(Bot bot, BotConfig config, TextGenerationRequestQueue requestQueue, CommandsProvider commandsProvider)
        {
            Config = config;
            Bot = bot;
            _requestQueue = requestQueue;
            _convertProvider = new InputConverterProvider();
            _commandsProvider = commandsProvider;
        }

        public void UpdateConfig(BotConfig config)
        {
            Config = config;
        }
        
        public void StartMessageReceiving()
        {
            _isReceiving = true;
            
            Bot.OnMessageReceived += OnMessageReceived;
            Bot.OnBotMessageForwarded += OnBotMessageForwarded;
        }

        public void StopMessageReceiving()
        {
            _isReceiving = false;
            
            Bot.OnMessageReceived -= OnMessageReceived;
            Bot.OnBotMessageForwarded -= OnBotMessageForwarded;
        }

        private void OnBotMessageForwarded(MessageData currentMessage, MessageData forwardedMessage)
        {
            Console.WriteLine($"Received message from: {currentMessage.Sender.UserName}");
            SendMessageRequest(currentMessage, currentMessage.Message);
        }

        private void OnMessageReceived(MessageData message)
        {
            Console.WriteLine($"Received message from: {message.Sender.UserName}");

            string formattedMessage = string.Empty;

            if (!string.IsNullOrEmpty(message.Message))
                formattedMessage = message.Message.Trim();
            
            if(_commandsProvider.TryExecuteCommand(message, this) || !string.IsNullOrEmpty(formattedMessage) && formattedMessage[0] == '/')
                return;
            
            string input = _convertProvider.Convert(message, Config);
            
            if(string.IsNullOrEmpty(input))
                return;
            
            SendMessageRequest(message, input);
        }

        private void SendMessageRequest(MessageData sourceMessage, string formattedResult)
        {
            var request = new ApiRequest(sourceMessage.Sender, sourceMessage.ChatData, formattedResult, this, OnRequestComplete);
            _requestQueue.AddRequestInQueue(request);
        }

        private void OnRequestComplete(string result, ChatData chat)
        {
            if(!_isReceiving)
                return;
            
            OnMessageGenerationComplete(result, chat);
        }

        protected abstract void OnMessageGenerationComplete(string botMessage, ChatData chat);

        public BotUserData GetUserData(UserData user)
        {
            return Data.GetUserData(user, Config);
        }

        public void UpdateUserData(UserData user, BotUserData data) => Data.UpdateUserData(user, data);
        
        public void LoadData()
        {
            if (SaveDataHandler.TryLoadData($"{Bot.Name}.bin", out BotData data))
                Data = data;
        }

        public void SaveData()
        {
            if(Data!=null)
                SaveDataHandler.SaveData($"{Bot.Name}.bin", Data);
        }
    }
}