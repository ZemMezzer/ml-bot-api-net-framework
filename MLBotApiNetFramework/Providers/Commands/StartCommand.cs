using System;
using ChatBotsApi.Core.Messages.Data;
using MLBotApiNetFramework.Bots;

namespace MLBotApiNetFramework.Providers.Commands
{
    public class StartCommand : ICommand
    {
        public bool TryExecute(MessageData message, ChatBot bot)
        {
            if (String.Equals(message.Message, "/Start", StringComparison.CurrentCultureIgnoreCase))
            {
                bot.GetUserData(message.Sender).History.Clear(bot.Config.InnerMessage);
                bot.Bot.SendTextMessage(bot.Config.InnerMessage, message.ChatData);
                return true;
            }

            return false;
        }
    }
}