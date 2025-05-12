using CSharpFunctionalExtensions;
using MediatR;
using NodaTime;

using Code.Example.Finances.Accounts;

namespace Code.Example.Finances.UseCases.AdminScope.Accounts
{
    public class RefillBalanceCommand : IRequest<UnitResult<Error>>
    {
        public AccountId AccountId { get; init; }
        public decimal Amount { get; init; }

        internal class Handler : IRequestHandler<RefillBalanceCommand, UnitResult<Error>>
        {
            private readonly IAccountRepository _accountRepository;
            private readonly IClock _clock;

            public Handler(
                IAccountRepository accountRepository,
                IClock clock)
            {
                _accountRepository = accountRepository;
                _clock = clock;
            }

            public async Task<UnitResult<Error>> Handle(RefillBalanceCommand command, CancellationToken ct)
            {
                var account = await _accountRepository.Find(command.AccountId, ct);

                return await UnitResult.SuccessIf(() => account != null, ErrorCodes.AccountNotFound)
                    .Tap(() => account!.RefillBalance(command.Amount, _clock.GetCurrentInstant()))
                    .Tap(() => _accountRepository.Update(account!, ct));
            }
        }
    }
}
