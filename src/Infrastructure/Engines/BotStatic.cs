using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Infrastructure.Engines;

public static class BotStatic
{
    private const string BotToken = "7655159692:AAHgfbMTgxVD1-Q56uwxvpK7q6Vq9Vtz9Kg";
    private static readonly TelegramBotClient bot = new(BotToken);

    public static void ReceiveMessages()
    {
        bot.OnMessage += OnMessage;
    }
    private static async Task OnMessage(Message msg, UpdateType type)
    {
        string content = string.Empty;

        if (msg.Type is MessageType.Text)
        {
            content = msg.Text ?? string.Empty;
        }

        if (msg.Type is MessageType.Photo)
        {
            content = msg.Caption ?? msg.Text ?? string.Empty;
        }

        if (content.Contains("Supply: 10,000,000 (+9 decimals)"))
        {
            await Console.Out.WriteLineAsync("match");
        }
    }
}
