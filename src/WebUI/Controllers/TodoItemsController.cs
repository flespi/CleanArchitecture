using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Application.TodoItems.Commands.DeleteTodoItem;
using CleanArchitecture.Application.TodoItems.Commands.UpdateTodoItem;
using CleanArchitecture.Application.TodoItems.Commands.UpdateTodoItemDetail;
using CleanArchitecture.Application.TodoItems.Queries.GetTodoItem;
using CleanArchitecture.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using CleanArchitecture.WebUI.Headers;
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
    public async Task<ActionResult<Guid>> Create(CreateTodoItemDto data, [FromHeader(Name = HeaderNames.IdempotencyKey)] Guid? idempotencyKey)
    {
        return await Mediator.Send(new CreateTodoItemCommand { Data = data });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDto>> Read(Guid id)
    {
        var response = await Mediator.Send(new GetTodoItemQuery { Id = id });

        Response.Headers.Add(HeaderNames.ETag, response.ConcurrencyToken.ToString());

        return response.Result!;
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Update(Guid id, UpdateTodoItemDto data, [FromHeader(Name = HeaderNames.IfMatch)] string concurrencyToken)
    {
        await Mediator.Send(new UpdateTodoItemCommand { Id = id, Data = data });

        return NoContent();
    }

    [HttpPut("{id}/details")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> UpdateItemDetails(Guid id, UpdateTodoItemDetailDto data, [FromHeader(Name = HeaderNames.IfMatch)] string concurrencyToken)
    {
        await Mediator.Send(new UpdateTodoItemDetailCommand { Id = id, Data = data });

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete(Guid id)
    {
        await Mediator.Send(new DeleteTodoItemCommand { Id = id });

        return NoContent();
    }
}
