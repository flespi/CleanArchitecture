using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Common.Models;

public class LookupDto<T>
{
    public T? Id { get; init; }

    public string? Title { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<TodoList, LookupDto<Guid>>();
            CreateMap<TodoItem, LookupDto<Guid>>();
        }
    }
}
