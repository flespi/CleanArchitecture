﻿using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Application.TodoItems.Commands.DeleteTodoItem;
using CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;
using CleanArchitecture.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace CleanArchitecture.Application.IntegrationTests.TodoItems.Commands;

using static Testing;

public class DeleteTodoItemTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidTodoItemId()
    {
        var command = new DeleteTodoItemCommand { Id = Guid.Empty };

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteTodoItem()
    {
        var listId = await SendAsync(new CreateTodoListCommand
        {
            Data = new()
            {
                Title = "New List"
            }
        });

        var itemId = await SendAsync(new CreateTodoItemCommand
        {
            Data = new()
            {
                ListId = listId,
                Title = "New Item"
            }
        });

        await SendAsync(new DeleteTodoItemCommand { Id = itemId });

        var item = await FindAsync<TodoItem>(itemId);

        item.Should().BeNull();
    }
}
