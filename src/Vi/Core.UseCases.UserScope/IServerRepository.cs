using Code.Example.Vi.Servers;

namespace Code.Example.Vi.UseCases.UserScope;

public interface IServerRepository
{
    Task<Server?> Find(ServerId serverId, CancellationToken ct = default);
    Task Update(Server server, CancellationToken ct = default);
}
