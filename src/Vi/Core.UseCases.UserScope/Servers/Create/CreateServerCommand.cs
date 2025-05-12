using CSharpFunctionalExtensions;
using MediatR;
using NodaTime;

namespace Code.Example.Vi.UseCases.UserScope.Servers.Create
{
    public class CreateServerCommand : IRequest<UnitResult<Error>>
    {
        public AccountId AccountId { get; init; }
        public decimal Amount { get; init; }

        internal class Handler : IRequestHandler<CreateServerCommand, UnitResult<Error>>
        {
            private readonly IServerRepository _accountRepository;
            private readonly IClock _clock;

            public Handler(
                IServerRepository accountRepository,
                IClock clock)
            {
                _accountRepository = accountRepository;
                _clock = clock;
            }

            public async Task<UnitResult<Error>> Handle(CreateServerCommand command, CancellationToken ct)
            {
                throw new NotImplementedException();
            }
        }
    }
}
