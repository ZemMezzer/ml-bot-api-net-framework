using ChatBotsApi.Core.Messages.Data;
using MLBotApiNetFramework.Bots;

namespace MLBotApiNetFramework.Providers.Commands
{
    public interface ICommand
    {
        public bool TryExecute(MessageData message, ChatBot bot);
    }
}