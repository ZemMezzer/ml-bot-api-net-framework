﻿using System;
using ChatBotsApi.Core.Messages.Data;
using NeuralNetworkBotApiNetFramework.Bots;

namespace NeuralNetworkBotApiNetFramework.Api.TextGeneration.Data
{
    public class ApiRequest
    {
        public string Input { get; }
        public ChatBot Bot { get; }
        public UserData Sender { get; }
        public ChatData Chat { get; }
        public event Action<string, ChatData> OnComplete;
        internal RequestData RequestData { get; }
        
        
        public ApiRequest(UserData sender, ChatData chat, string input, ChatBot bot, Action<string, ChatData> onComplete)
        {
            Bot = bot;
            Sender = sender;
            Chat = chat;
            Input = input;
            OnComplete = onComplete;
            RequestData = new RequestData(input, bot.Bot.Name, sender.UserName, bot.GetUserHistory(sender));
        }

        internal void OnRequestCompleted(string result)
        {
            OnComplete?.Invoke(result, Chat);
        }
    }
}