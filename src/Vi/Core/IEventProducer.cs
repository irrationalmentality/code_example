namespace Code.Example.Vi;

public interface IEventProducer
{
    Queue<IEvent> Events { get; }
}
