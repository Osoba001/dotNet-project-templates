using Auth.Application.RepositoryContracts;
using Utilities.Responses;

namespace Auth.Application.MediatR
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task<KOActionResult> HandleAsync(TCommand command, IServiceWrapper service, CancellationToken cancellationToken = default);
    }
}
