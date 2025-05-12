using NodaTime;

namespace Code.Example.Finances.Accounts;

public class AccountPaid : IEvent
{
    public string Type => "account_paid";
    public AccountId AccountId { get; }
    public decimal Amount { get; }
    public Instant Date { get; }

    public AccountPaid(AccountId accountId, decimal amount, Instant paidAt)
    {
        AccountId = accountId;
        Amount = amount;
        Date = paidAt;
    }
}
