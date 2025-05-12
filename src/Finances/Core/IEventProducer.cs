namespace Code.Example.Finances;

public interface IEventProducer
{
    Queue<IEvent> Events { get; }
}
