using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Application.TodoLists.Commands.DeleteTodoList;
using CleanArchitecture.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace CleanArchitecture.Application.IntegrationTests.TodoLists.Commands;

public class DeleteTodoListTests : BaseTest
{
    public DeleteTodoListTests(TestContext context) : base(context)
    {
    }

    [Fact]
    public async Task ShouldRequireValidTodoListId()
    {
        var command = new DeleteTodoListCommand(99);
        await FluentActions.Invoking(() => Context.SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task ShouldDeleteTodoList()
    {
        var listId = await Context.SendAsync(new CreateTodoListCommand
        {
            Title = "New List"
        });

        await Context.SendAsync(new DeleteTodoListCommand(listId));

        var list = await Context.FindAsync<TodoList>(listId);

        list.Should().BeNull();
    }
}
