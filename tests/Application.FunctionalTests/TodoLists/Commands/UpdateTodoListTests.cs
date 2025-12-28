using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Application.TodoLists.Commands.UpdateTodoList;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.FunctionalTests.TodoLists.Commands;

using static Testing;

public class UpdateTodoListTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidTodoListId()
    {
        var command = new UpdateTodoListCommand
        {
            Id = 99,
            Data = new()
            {
                Title = "New Title"
            }
        };
        
        await Should.ThrowAsync<NotFoundException>(() => SendAsync(command));
    }

    [Test]
    public async Task ShouldRequireUniqueTitle()
    {
        var listId = await SendAsync(new CreateTodoListCommand
        {
            Data = new()
            {
                Title = "New List"
            }
        });

        await SendAsync(new CreateTodoListCommand
        {
            Data = new()
            {
                Title = "Other List"
            }
        });

        var command = new UpdateTodoListCommand
        {
            Id = listId,
            Data = new()
            {
                Title = "Other List"
            }
        };

        var ex = await Should.ThrowAsync<ValidationException>(() => SendAsync(command));

        ex.Errors.ShouldContainKey("Data.Title");
        ex.Errors["Data.Title"].ShouldContain("'Title' must be unique.");
    }

    [Test]
    public async Task ShouldUpdateTodoList()
    {
        var userId = await RunAsDefaultUserAsync();

        var listId = await SendAsync(new CreateTodoListCommand
        {
            Data = new()
            {
                Title = "New List"
            }
        });

        var command = new UpdateTodoListCommand
        {
            Id = listId,
            Data = new()
            {
                Title = "Updated List Title"
            }
        };

        await SendAsync(command);

        var list = await FindAsync<TodoList>(listId);

        list.ShouldNotBeNull();
        list!.Title.ShouldBe(command.Data.Title);
        list.LastModifiedBy.ShouldNotBeNull();
        list.LastModifiedBy.ShouldBe(userId);
        list.LastModified.ShouldBe(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
