using CleanArchitecture.Application.Common.Cqrs;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Transactions;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using MediatR;

namespace CleanArchitecture.Application.TodoItems.Commands.UpdateTodoItemDetail;

[Transactional]
public record UpdateTodoItemDetailCommand : UpdateCommand<UpdateTodoItemDetailDto>;

public class UpdateTodoItemDetailCommandHandler : IRequestHandler<UpdateTodoItemDetailCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTodoItemDetailCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateTodoItemDetailCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoItems
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(TodoItem), request.Id);
        }

        if (!request.ConcurrencyToken?.Equals(entity.ConcurrencyToken!) ?? false)
        {
            throw new ConcurrencyException();
        }

        entity.ListId = request.Data!.ListId;
        entity.Priority = request.Data!.Priority;
        entity.Note = request.Data!.Note;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
