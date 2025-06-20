using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.TodoItems.Commands.UpdateTodoItem;

public record UpdateTodoItemCommand : IRequest
{
    public int Id { get; init; }

    public string? Title { get; init; }

    public bool Done { get; init; }
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

        Guard.Against.NotFound(request.Id, entity);

        if (!_conditions.IfMatch?.Equals(entity.ConcurrencyToken) ?? false)
        {
            throw new ConcurrencyException();
        }

        entity.Title = request.Title;
        entity.Done = request.Done;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
