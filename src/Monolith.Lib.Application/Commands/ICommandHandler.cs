using Monolith.Lib.Primitives;

namespace Monolith.Lib.Application.Commands;

public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
{
    Task<Result<TResult>> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}