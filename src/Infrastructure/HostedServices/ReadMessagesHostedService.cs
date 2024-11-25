using Infrastructure.Engines;
using Infrastructure.Entities;
using Infrastructure.Extensions;
using Infrastructure.Persistent;
using Infrastructure.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Npgsql;
using WTelegram;

namespace Infrastructure.HostedServices;

public class ReadMessagesHostedService : BackgroundService
{
    private readonly IServiceScopeFactory serviceScopeFactory;
    private readonly TelegramSettings telegramSettings;
    private readonly DatabaseSettings databaseSettings;

    public ReadMessagesHostedService(IServiceScopeFactory serviceScopeFactory, IOptions<TelegramSettings> telegramOptions, IOptions<DatabaseSettings> databaseOptions)
    {
        this.serviceScopeFactory = serviceScopeFactory;
        telegramSettings = telegramOptions.Value;
        databaseSettings = databaseOptions.Value;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //BotStatic.ReceiveMessages();
        //return;
        IWBotEngine engine = serviceScopeFactory.GetService<IWBotEngine>();

        using NpgsqlConnection connection = new(databaseSettings.ConnectionString);
        using Bot bot = new(telegramSettings.BotToken, telegramSettings.AppId, telegramSettings.AppHash, connection);
        ChannelConfig channel = ChannelConfig.Default;
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await engine.ReadLastedMessagesAsync(channel, bot);
                System.Console.WriteLine("------ Add logging -------------");
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Exception: " + ex.Message);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}
