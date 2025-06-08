using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.TodoItems.Commands.UpdateTodoItemDetail;

public record UpdateTodoItemDetailCommand : IRequest
{
    public int Id { get; init; }

    public UpdateTodoItemDetailDto? Data { get; init; }
}

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

        Guard.Against.NotFound(request.Id, entity);

        entity.ListId = request.Data!.ListId;
        entity.Priority = request.Data!.Priority;
        entity.Note = request.Data!.Note;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
