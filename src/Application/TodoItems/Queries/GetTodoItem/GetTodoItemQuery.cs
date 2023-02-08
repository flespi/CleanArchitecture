using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Domain.Entities;
using MediatR;

namespace CleanArchitecture.Application.TodoItems.Queries.GetTodoItem;

public record GetTodoItemQuery : IRequest<Versioned<TodoItemDto>>
{
    public Guid Id { get; init; }
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

        if (entity == null)
        {
            throw new NotFoundException(nameof(TodoItem), request.Id);
        }

        var result = _mapper.Map<TodoItemDto>(entity);

        return Versioned.FromResult(result, entity.ConcurrencyToken!);
    }
}
