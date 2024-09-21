using Infrastructure.Entities;
using Infrastructure.Persistent;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Npgsql;
using Telegram.Bot.Types.Enums;
using WTelegram;
using WTelegram.Types;

namespace Infrastructure.Engines;

public interface IWBotEngine
{
    Task ReadLastedMessagesAsync(ChannelConfig channel);
}

public class WBotEngine : IWBotEngine
{
    private readonly TelegramSettings telegramSettings;
    private readonly DatabaseSettings databaseSettings;
    public WBotEngine(IOptions<TelegramSettings> telegramOptions, IOptions<DatabaseSettings> databaseOptions)
    {
        telegramSettings = telegramOptions.Value;
        databaseSettings = databaseOptions.Value;
    }

    private const string ForwardId = "-4531896172";

    public async Task ReadLastedMessagesAsync(ChannelConfig channel)
    {
        using NpgsqlConnection connection = new(databaseSettings.ConnectionString);
        using Bot bot = new(telegramSettings.BotToken, telegramSettings.AppId, telegramSettings.AppHash, connection);
        List<Message> messages = await bot.GetMessagesById(channel.Id, Enumerable.Range(channel.ReadMessageId, 10));
        foreach (Message message in messages)
        {
            await Console.Out.WriteLineAsync(" Current message read: " + message.MessageId);
            channel.ReadMessageId = message.MessageId;
            MessageType type = message.Type;
            if (type is MessageType.Unknown)
            {
                return;
            }

            string content = message.Caption ?? message.Text ?? string.Empty;
            if (content == null)
            {
                continue;
            }

        }
    }
}
