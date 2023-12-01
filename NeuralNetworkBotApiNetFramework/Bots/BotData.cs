using System;
using System.Collections.Generic;
using ChatBotsApi.Core.Messages.Data;

namespace NeuralNetworkBotApiNetFramework.Bots;

[Serializable]
public class BotData
{
    private Dictionary<UserData, BotUserData> _usersData = new();
    public IReadOnlyDictionary<UserData, BotUserData> UsersData => _usersData;
    
    public BotUserData GetUserData(UserData user, BotConfig config)
    {
        if(!_usersData.ContainsKey(user))
            _usersData.Add(user, new BotUserData(config.InnerMessage, config));

        return _usersData[user];
    }

    public void UpdateUserData(UserData user, BotUserData data)
    {
        if(!_usersData.ContainsKey(user))
            _usersData.Add(user, default);

        _usersData[user] = data;
    }
}