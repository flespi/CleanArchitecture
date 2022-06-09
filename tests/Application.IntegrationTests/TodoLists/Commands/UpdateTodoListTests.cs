using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Application.TodoLists.Commands.UpdateTodoList;
using CleanArchitecture.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace CleanArchitecture.Application.IntegrationTests.TodoLists.Commands;

using static Testing;

public class UpdateTodoListTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidTodoListId()
    {
        var command = new UpdateTodoListCommand
        {
            Id = Guid.Empty,
            Data = new()
            {
                Title = "New Title"
            }
        };
        
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
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

        (await FluentActions.Invoking(() =>
            SendAsync(command))
                .Should().ThrowAsync<ValidationException>().Where(ex => ex.Errors.ContainsKey("Data.Title")))
                .And.Errors["Data.Title"].Should().Contain("Title must be unique.");
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

        list.Should().NotBeNull();
        list!.Title.Should().Be(command.Data.Title);
        list.Audit.LastModifiedBy.Should().NotBeNull();
        list.Audit.LastModifiedBy.Should().Be(userId);
        list.Audit.LastModified.Should().NotBeNull();
        list.Audit.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
