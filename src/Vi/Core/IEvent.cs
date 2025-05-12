using NodaTime;

namespace Code.Example.Vi;

public interface IEvent
{
    string Type { get; }

    Instant Date { get; }
}
