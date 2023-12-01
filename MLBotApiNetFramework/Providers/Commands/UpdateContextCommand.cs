using ChatBotsApi.Core.Messages.Data;
using MLBotApiNetFramework.Bots;

namespace MLBotApiNetFramework.Providers.Commands;

public class UpdateContextCommand : ICommand
{
    public bool TryExecute(MessageData message, ChatBot bot)
    {
        if (!message.Message.ToLower().StartsWith("/UpdateContext".ToLower()))
            return false;
        
        string context = message.Message.Replace("/UpdateContext", "");
        
        var userData = bot.GetUserData(message.Sender);
        userData.UserContext = context;
        bot.UpdateUserData(message.Sender, userData);

        return true;
    }
}