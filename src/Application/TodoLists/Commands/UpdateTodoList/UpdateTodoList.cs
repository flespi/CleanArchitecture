using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.TodoLists.Commands.UpdateTodoList;

public record UpdateTodoListCommand : IRequest
{
    public int Id { get; init; }

    public string? Title { get; init; }
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

        Guard.Against.NotFound(request.Id, entity);

        if (!_conditions.IfMatch?.Equals(entity.ConcurrencyToken) ?? false)
        {
            throw new ConcurrencyException();
        }

        entity.Title = request.Title;

        await _context.SaveChangesAsync(cancellationToken);

    }
}
