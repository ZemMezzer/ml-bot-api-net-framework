using System;
using ChatBotsApi.Core.Messages.Data;
using NeuralNetworkBotApiNetFramework.Bots;

namespace NeuralNetworkBotApiNetFramework.Providers.Commands
{
    public class StartCommand : ICommand
    {
        public bool TryExecute(MessageData message, ChatBot bot)
        {
            if (String.Equals(message.Message, "/Start", StringComparison.CurrentCultureIgnoreCase))
            {
                bot.GetUserHistory(message.Sender).Clear(bot.Config.InnerMessage);
                bot.Bot.SendTextMessage(bot.Config.InnerMessage, message.ChatData);
                return true;
            }

            return false;
        }
    }
}