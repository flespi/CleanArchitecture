using CleanArchitecture.Application.Common.Cqrs;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;

namespace CleanArchitecture.Application.TodoItems.Commands.UpdateTodoItem;

public record UpdateTodoItemCommand : UpdateCommand<UpdateTodoItemDto>;

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateTodoItemCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
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

        entity.Title = request.Data!.Title;
        entity.Done = request.Data!.Done;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
