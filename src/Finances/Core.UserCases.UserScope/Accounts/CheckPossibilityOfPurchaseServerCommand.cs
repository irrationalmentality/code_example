using CSharpFunctionalExtensions;
using MediatR;
using NodaTime;

using Code.Example.Finances;
using Code.Example.Finances.Accounts;
using Code.Example.Finances.Tariffs;

namespace Core.UserCases.UserScope.Accounts
{
    public class CheckPossibilityOfPurchaseServerCommand : IRequest<UnitResult<Error>>
    {
        public AccountId AccountId { get; init; }
        public CpuCount CpuCount { get; init; } = null!;
        public RamSize RamSize { get; init; } = null!;

        internal class Handler : IRequestHandler<PayServerCommand, UnitResult<Error>>
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

            public async Task<UnitResult<Error>> Handle(PayServerCommand command, CancellationToken ct)
            {
                throw new NotImplementedException();
            }
        }
    }
}
