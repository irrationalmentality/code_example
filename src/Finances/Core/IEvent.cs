using NodaTime;

namespace Code.Example.Finances;

public interface IEvent
{
    string Type { get; }

    Instant Date { get; }
}
