﻿using System.Collections.Generic;
using System.Linq;
using ChatBotsApi.Core.Messages.Data;
using NeuralNetworkBotApiNetFramework.Bots;
using NeuralNetworkBotApiNetFramework.Providers.Commands;

namespace NeuralNetworkBotApiNetFramework.Providers
{
    public class CommandsProvider
    {
        private readonly List<ICommand> _commands;

        public CommandsProvider(IReadOnlyList<ICommand> commands) => _commands = commands.ToList();

        public bool TryExecuteCommand(MessageData message, ChatBot bot)
        {
            foreach (var command in _commands)
            {
                if (command.TryExecute(message, bot))
                    return true;
            }

            return false;
        }
    }
}