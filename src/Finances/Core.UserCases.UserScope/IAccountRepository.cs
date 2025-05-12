using Code.Example.Finances.Accounts;

namespace Core.UserCases.UserScope;

public interface IAccountRepository
{
    Task<Account?> Find(AccountId accountId, CancellationToken ct = default);
    Task Update(Account account, CancellationToken ct = default);
}
