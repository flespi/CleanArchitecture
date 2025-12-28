using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.FunctionalTests.TodoItems.Commands;

using static Testing;

public class CreateTodoItemTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateTodoItemCommand();

        await Should.ThrowAsync<ValidationException>(() => SendAsync(command));
    }

    [Test]
    public async Task ShouldCreateTodoItem()
    {
        var userId = await RunAsDefaultUserAsync();

        var listId = await SendAsync(new CreateTodoListCommand
        {
            Title = "New List"
        });

        var command = new CreateTodoItemCommand
        {
            ListId = listId,
            Title = "Tasks"
        };

        var itemId = await SendAsync(command);

        var item = await FindAsync<TodoItem>(itemId);

        item.ShouldNotBeNull();
        item!.ListId.ShouldBe(command.ListId);
        item.Title.ShouldBe(command.Title);
        item.Audit.CreatedBy.ShouldBe(userId);
        item.Audit.Created.ShouldBe(DateTime.Now, TimeSpan.FromMilliseconds(10000));
        item.Audit.LastModifiedBy.ShouldBe(userId);
        item.Audit.LastModified.ShouldBe(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
