using CleanArchitecture.Application.Common.Cqrs;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Transactions;
using CleanArchitecture.Application.TodoItems.Commands.UpdateTodoItemDetail;
using CleanArchitecture.Domain.Entities;
using MediatR;

namespace CleanArchitecture.Application.TodoItems.Commands.UpdateTodoItem;

[Transactional]
public record UpdateTodoItemCommand : IRequest
{
    public required Guid Id { get; init; }

    public required UpdateTodoItemDto Data { get; init; }
}

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateTodoItemCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IConditionalParameters _conditions;

    public UpdateTodoItemCommandHandler(IApplicationDbContext context, IConditionalParameters conditions)
    {
        _context = context;
        _conditions = conditions;
    }

    public async Task Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoItems
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(TodoItem), request.Id);
        }

        if (!_conditions.IfMatch?.Equals(entity.ConcurrencyToken!) ?? false)
        {
            throw new ConcurrencyException();
        }

        entity.Title = request.Data.Title;
        entity.Done = request.Data.Done;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
