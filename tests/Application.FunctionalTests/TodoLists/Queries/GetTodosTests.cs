using CleanArchitecture.Application.TodoLists.Queries.GetTodos;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.ValueObjects;

namespace CleanArchitecture.Application.FunctionalTests.TodoLists.Queries;

public class GetTodosTests : BaseTest
{
    public GetTodosTests(TestContext context) : base(context)
    {
    }

    [Fact]
    public async Task ShouldReturnPriorityLevels()
    {
        await RunAsDefaultUserAsync();

        var query = new GetTodosQuery();

        var result = await SendAsync(query);

        result.PriorityLevels.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task ShouldReturnAllListsAndItems()
    {
        await RunAsDefaultUserAsync();

        await AddAsync(new TodoList
        {
            Title = "Shopping",
            Colour = Colour.Blue,
            Items =
                {
                    new TodoItem { Title = "Apples", Done = true },
                    new TodoItem { Title = "Milk", Done = true },
                    new TodoItem { Title = "Bread", Done = true },
                    new TodoItem { Title = "Toilet paper" },
                    new TodoItem { Title = "Pasta" },
                    new TodoItem { Title = "Tissues" },
                    new TodoItem { Title = "Tuna" }
                }
        });

        var query = new GetTodosQuery();

        var result = await SendAsync(query);

        result.Lists.Count.ShouldBe(1);
        result.Lists.First().Items.Count.ShouldBe(7);
    }

    [Fact]
    public async Task ShouldDenyAnonymousUser()
    {
        var query = new GetTodosQuery();

        var action = () => SendAsync(query);

        await Should.ThrowAsync<UnauthorizedAccessException>(action);
    }
}
