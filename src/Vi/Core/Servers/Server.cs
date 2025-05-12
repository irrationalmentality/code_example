using NodaTime;

namespace Code.Example.Vi.Servers;

public class Server
{
    public ServerId Id { get; private set; }

    public AccountId AccountId { get; private set; }

    public CpuCount CpuCount { get; private set; }

    public RamSize RamSize { get; private set; }

    public long ConcurrencyToken { get; private set; }

    public Queue<IEvent> Events { get; } = new();

    public Server() { } // ef leak

    public Server(
        ServerId id,
        AccountId accountId,
        CpuCount cpuCount,
        RamSize ramSize,
        Instant createdAt)
    {
        Id = id;
        AccountId = accountId;
        CpuCount = cpuCount;
        RamSize = ramSize;

        Events.Enqueue(new ServerCreated(Id, accountId, createdAt));
    }
}
