using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Transactions;
using CleanArchitecture.Domain.Entities;
using MediatR;

namespace CleanArchitecture.Application.TodoItems.Commands.UpdateTodoItemDetail;

[Transactional]
public record UpdateTodoItemDetailCommand : IRequest
{
    public required Guid Id { get; init; }

    public required UpdateTodoItemDetailDto Data { get; init; }
}

public class UpdateTodoItemDetailCommandHandler : IRequestHandler<UpdateTodoItemDetailCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IConditionalParameters _conditions;

    public UpdateTodoItemDetailCommandHandler(IApplicationDbContext context, IConditionalParameters conditions)
    {
        _context = context;
        _conditions = conditions;
    }

    public async Task Handle(UpdateTodoItemDetailCommand request, CancellationToken cancellationToken)
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

        entity.ListId = request.Data.ListId;
        entity.Priority = request.Data.Priority;
        entity.Note = request.Data.Note;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
