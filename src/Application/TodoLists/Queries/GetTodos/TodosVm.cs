using CleanArchitecture.Application.Common.Models;

namespace CleanArchitecture.Application.TodoLists.Queries.GetTodos;

public class TodosVm
{
    public IReadOnlyCollection<LookupDto<int>> PriorityLevels { get; init; } = Array.Empty<LookupDto<int>>();

    public IReadOnlyCollection<TodoListDto> Lists { get; init; } = Array.Empty<TodoListDto>();
}
