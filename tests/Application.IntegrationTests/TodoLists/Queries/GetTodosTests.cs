using CleanArchitecture.Application.TodoLists.Queries.GetTodos;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace CleanArchitecture.Application.IntegrationTests.TodoLists.Queries;

public class GetTodosTests : BaseTest
{
    public GetTodosTests(TestContext context) : base(context)
    {
    }

    [Fact]
    public async Task ShouldReturnPriorityLevels()
    {
        await Context.RunAsDefaultUserAsync();

        var query = new GetTodosQuery();

        var result = await Context.SendAsync(query);

        result.PriorityLevels.Should().NotBeEmpty();
    }

    [Fact]
    public async Task ShouldReturnAllListsAndItems()
    {
        await Context.RunAsDefaultUserAsync();

        await Context.AddAsync(new TodoList
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

        var result = await Context.SendAsync(query);

        result.Lists.Should().HaveCount(1);
        result.Lists.First().Items.Should().HaveCount(7);
    }

    [Fact]
    public async Task ShouldDenyAnonymousUser()
    {
        var query = new GetTodosQuery();

        var action = () => Context.SendAsync(query);
        
        await action.Should().ThrowAsync<UnauthorizedAccessException>();
    }
}
