﻿using ChatBotsApi.Bots.TelegramBot;
using ChatBotsApi.Core.Messages.Data;
using MLBotApiNetFramework.Api.TextGeneration;
using MLBotApiNetFramework.Providers;

namespace MLBotApiNetFramework.Bots
{
    public class TelegramChatBot : ChatBot
    {
        public TelegramChatBot(BotConfig config, TextGenerationRequestQueue requestQueue, CommandsProvider commandsProvider) : base(new TelegramBot(config.BotToken, config.Name, config.BotId), config, requestQueue, commandsProvider) {}

        protected override void OnMessageGenerationComplete(string botMessage, ChatData chat)
        {
            Bot.SendTextMessage(botMessage, chat);
        }
    }
}