using System;
using ChatBotsApi.Core.Messages.Data;
using NeuralNetworkBotApiNetFramework.Bots;

namespace NeuralNetworkBotApiNetFramework.Providers.Commands
{
    public class ClearCommand : ICommand
    {
        public bool TryExecute(MessageData message, ChatBot bot)
        {
            var history = bot.GetUserData(message.Sender).History;
            
            if (String.Equals(message.Message, "/Clear", StringComparison.CurrentCultureIgnoreCase))
            {
                history.Clear(bot.Config.InnerMessage);
                return true;
            }

            return false;
        }
    }
}