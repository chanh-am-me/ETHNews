namespace Infrastructure.Entities;

public class ChannelConfig
{
    public static readonly ChannelConfig Default = GetDefault();

    public string Id { get; set; } = default!;

    public int ReadMessageId { get; set; }

    private static ChannelConfig GetDefault()
    {
        return new()
        {
            Id = "@iTokenEthereum",
            ReadMessageId = 499613,
        };
    }
}
