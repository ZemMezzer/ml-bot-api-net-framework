using ChatBotsApi.Core.Messages.Data;
using MLBotApiNetFramework.Bots;

namespace MLBotApiNetFramework.Providers
{
    public class InputConverterProvider
    {
        public string Convert(MessageData message, BotConfig config)
        {
            string lowerMessage = message.Message.ToLower();

            if (message.IsPrivateMessage)
                return message.Message;
            
            if (message.Message.Contains($"@{config.BotId}"))
                return message.Message.Replace($"@{config.BotId}", "");

            foreach (var alias in config.NameAliases)
            {
                if (lowerMessage.Contains(alias.ToLower()))
                    return message.Message;
            }

            return string.Empty;
        }
    }
}