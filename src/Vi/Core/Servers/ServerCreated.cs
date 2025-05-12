using NodaTime;

namespace Code.Example.Vi.Servers;

public class ServerCreated : IEvent
{
    public string Type => "server_created";
    public ServerId ServerId { get; }
    public AccountId AccountId { get; }
    public Instant Date { get; }

    public ServerCreated(ServerId serverId, AccountId accountId, Instant paidAt)
    {
        ServerId = serverId;
        AccountId = accountId;
        Date = paidAt;
    }
}
