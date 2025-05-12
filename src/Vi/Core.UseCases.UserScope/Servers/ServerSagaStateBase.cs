namespace Code.Example.Vi.UseCases.UserScope.Servers;

public abstract class ServerSagaStateBase
{
    public ServerSagaId Id { get; private set; }

    protected ServerSagaStateBase() { } // ef leak

    protected ServerSagaStateBase(ServerSagaId id)
    {
        Id = id;
    }
}
