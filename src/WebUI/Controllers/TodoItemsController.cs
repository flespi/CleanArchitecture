using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Application.TodoItems.Commands.DeleteTodoItem;
using CleanArchitecture.Application.TodoItems.Commands.UpdateTodoItem;
using CleanArchitecture.Application.TodoItems.Commands.UpdateTodoItemDetail;
using CleanArchitecture.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUI.Controllers;

[Authorize]
public class TodoItemsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<TodoItemBriefDto>>> GetTodoItemsWithPagination([FromQuery] GetTodoItemsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateTodoItemDto data)
    {
        return await Mediator.Send(new CreateTodoItemCommand { Data = data });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateTodoItemDto data)
    {
        await Mediator.Send(new UpdateTodoItemCommand { Id = id, Data = data });

        return NoContent();
    }

    [HttpPut("{id}/details")]
    public async Task<ActionResult> UpdateItemDetails(int id, UpdateTodoItemDetailDto data)
    {
        await Mediator.Send(new UpdateTodoItemDetailCommand { Id = id, Data = data });

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteTodoItemCommand { Id = id });

        return NoContent();
    }
}
