using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Transactions;
using CleanArchitecture.Domain.Entities;
using MediatR;

namespace CleanArchitecture.Application.TodoLists.Commands.UpdateTodoList;

[Transactional]
public record UpdateTodoListCommand : IRequest
{
    public required Guid Id { get; init; }

    public required UpdateTodoListDto Data { get; init; }
}

public class UpdateTodoListCommandHandler : IRequestHandler<UpdateTodoListCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IConditionalParameters _conditions;

    public UpdateTodoListCommandHandler(IApplicationDbContext context, IConditionalParameters conditions)
    {
        _context = context;
        _conditions = conditions;
    }

    public async Task Handle(UpdateTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoLists
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(TodoList), request.Id);
        }

        if (!_conditions.IfMatch?.Equals(entity.ConcurrencyToken!) ?? false)
        {
            throw new ConcurrencyException();
        }

        entity.Title = request.Data.Title;

        await _context.SaveChangesAsync(cancellationToken);

    }
}
