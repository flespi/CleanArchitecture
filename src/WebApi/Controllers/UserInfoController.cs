using CleanArchitecture.Application.UserInfo.Commands.RefreshUserInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebApi.Controllers;

[Authorize]
public class UserInfoController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Refresh()
    {
        await Mediator.Send(new RefreshUserInfoCommand());
        return NoContent();
    }
}
