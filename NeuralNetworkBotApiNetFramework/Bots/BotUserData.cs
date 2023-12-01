using System;
using NeuralNetworkBotApiNetFramework.Api.TextGeneration.Data;

namespace NeuralNetworkBotApiNetFramework.Bots;

[Serializable]
public struct BotUserData
{
    public string Name;
    public string UserContext;
    public History History;

    public BotUserData(string userName, BotConfig config)
    {
        Name = userName;
        History = new History(config.InnerMessage);
    }
}