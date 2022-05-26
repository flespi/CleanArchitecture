using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Application.TodoLists.Commands.PurgeTodoLists;
using CleanArchitecture.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace CleanArchitecture.Application.IntegrationTests.TodoLists.Commands;

public class PurgeTodoListsTests : BaseTest
{
    public PurgeTodoListsTests(TestContext context) : base(context)
    {
    }

    [Fact]
    public async Task ShouldDenyAnonymousUser()
    {
        var command = new PurgeTodoListsCommand();

        command.GetType().Should().BeDecoratedWith<AuthorizeAttribute>();

        var action = () => Context.SendAsync(command);

        await action.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task ShouldDenyNonAdministrator()
    {
        await Context.RunAsDefaultUserAsync();

        var command = new PurgeTodoListsCommand();

        var action = () => Context.SendAsync(command);

        await action.Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Fact]
    public async Task ShouldAllowAdministrator()
    {
        await Context.RunAsAdministratorAsync();

        var command = new PurgeTodoListsCommand();

        var action = () => Context.SendAsync(command);

        await action.Should().NotThrowAsync<ForbiddenAccessException>();
    }

    [Fact]
    public async Task ShouldDeleteAllLists()
    {
        await Context.RunAsAdministratorAsync();

        await Context.SendAsync(new CreateTodoListCommand
        {
            Title = "New List #1"
        });

        await Context.SendAsync(new CreateTodoListCommand
        {
            Title = "New List #2"
        });

        await Context.SendAsync(new CreateTodoListCommand
        {
            Title = "New List #3"
        });

        await Context.SendAsync(new PurgeTodoListsCommand());

        var count = await Context.CountAsync<TodoList>();

        count.Should().Be(0);
    }
}
