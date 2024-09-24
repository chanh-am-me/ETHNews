using Infrastructure.Entities;
using Infrastructure.Persistent;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types.Enums;
using WTelegram;
using WTelegram.Types;

namespace Infrastructure.Engines;

public interface IWBotEngine
{
    Task ReadLastedMessagesAsync(ChannelConfig channel, Bot bot);
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

    private const string ForwardId = "-1002276561141";

    public async Task ReadLastedMessagesAsync(ChannelConfig channel, Bot bot)
    {
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

            if (content.Contains("Supply: 10,000,000 (+9 decimals)") || content.Contains("Supply: 100,000,000 (+9 decimals)"))
            {
                Message forward = await bot.ForwardMessage(ForwardId, channel.Id, message.MessageId, message.MessageId);
                await bot.SendTextMessage(ForwardId, "Nghi VIT", replyParameters: forward);
            }
        }
    }
}
