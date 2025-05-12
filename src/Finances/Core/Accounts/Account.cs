using CSharpFunctionalExtensions;
using NodaTime;

namespace Code.Example.Finances.Accounts;

public class Account : IEventProducer
{
    public AccountId Id { get; private set; }

    public decimal Amount { get; private set; }

    public long ConcurrencyToken { get; private set; }

    public Queue<IEvent> Events { get; } = new();

    public Account() { } // ef leak

    public Account(AccountId id)
    {
        Id = id;
    }

    public void RefillBalance(decimal amount, Instant replenishedAt)
    {
        Amount += amount;

        Events.Enqueue(new AccountReplenished(Id, amount, replenishedAt));

        IncrementVersion();
    }

    public bool IsEnoughFunds(decimal amount)
        => Amount >= amount;

    public UnitResult<Error> Pay(decimal amount, Instant paidAt)
    {
        return UnitResult
            .FailureIf(Amount < amount, ErrorCodes.InsufficientFunds)
            .Tap(() => Amount -= amount)
            .Tap(() => Events.Enqueue(new AccountPaid(Id, amount, paidAt)))
            .Tap(IncrementVersion);
    }

    private void IncrementVersion()
        => ConcurrencyToken++;
}
