using NodaTime;

namespace Code.Example.Finances.Accounts;

public class AccountReplenished : IEvent
{
    public string Type => "account_replenished";
    public AccountId AccountId { get; }
    public decimal Amount { get; }
    public Instant Date { get; }

    public AccountReplenished(AccountId accountId, decimal amount, Instant replenishedAt)
    {
        AccountId = accountId;
        Amount = amount;
        Date = replenishedAt;
    }
}
