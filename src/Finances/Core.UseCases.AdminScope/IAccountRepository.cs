using Code.Example.Finances.Accounts;

namespace Code.Example.Finances.UseCases.AdminScope;

public interface IAccountRepository
{
    Task<Account?> Find(AccountId accountId, CancellationToken ct = default);
    Task Update(Account account, CancellationToken ct = default);
}
