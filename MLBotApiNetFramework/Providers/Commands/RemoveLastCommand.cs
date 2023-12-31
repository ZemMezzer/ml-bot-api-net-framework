﻿using System;
using ChatBotsApi.Core.Messages.Data;
using MLBotApiNetFramework.Bots;

namespace MLBotApiNetFramework.Providers.Commands
{
    public class RemoveLastCommand : ICommand
    {
        public bool TryExecute(MessageData message, ChatBot bot)
        {
            var history = bot.GetUserData(message.Sender).History;
            
            if (String.Equals(message.Message, "/RemoveLast", StringComparison.CurrentCultureIgnoreCase))
            {
                history.RemoveLast();
                return true;
            }

            return false;
        }
    }
}