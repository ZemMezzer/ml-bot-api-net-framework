using ChatBotsApi.Core.Messages.Data;
using NeuralNetworkBotApiNetFramework.Bots;

namespace NeuralNetworkBotApiNetFramework.Providers.Commands
{
    public interface ICommand
    {
        public bool TryExecute(MessageData message, ChatBot bot);
    }
}