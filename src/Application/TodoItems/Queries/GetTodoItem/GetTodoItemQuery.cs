using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;

namespace CleanArchitecture.Application.TodoItems.Queries.GetTodoItem;

public record GetTodoItemQuery : IRequest<Versioned<TodoItemDto>>
{
    public int Id { get; init; }
}

public class GetTodoItemsWithPaginationQueryHandler : IRequestHandler<GetTodoItemQuery, Versioned<TodoItemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTodoItemsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Versioned<TodoItemDto>> Handle(GetTodoItemQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoItems
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        return new Versioned<TodoItemDto>
        {
            Result = _mapper.Map<TodoItemDto>(entity),
            ConcurrencyToken = entity.ConcurrencyToken
        };
    }
}
