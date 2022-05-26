using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Application.TodoItems.Commands.DeleteTodoItem;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace CleanArchitecture.Application.IntegrationTests.TodoItems.Commands;

public class DeleteTodoItemTests : BaseTest
{
    public DeleteTodoItemTests(TestContext context) : base(context)
    {
    }

    [Fact]
    public async Task ShouldRequireValidTodoItemId()
    {
        var command = new DeleteTodoItemCommand(99);

        await FluentActions.Invoking(() =>
            Context.SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task ShouldDeleteTodoItem()
    {
        var listId = await Context.SendAsync(new CreateTodoListCommand
        {
            Title = "New List"
        });

        var itemId = await Context.SendAsync(new CreateTodoItemCommand
        {
            ListId = listId,
            Title = "New Item"
        });

        await Context.SendAsync(new DeleteTodoItemCommand(itemId));

        var item = await Context.FindAsync<TodoItem>(itemId);

        item.Should().BeNull();
    }
}
