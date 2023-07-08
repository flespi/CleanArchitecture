using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.WebUI.Headers;

namespace CleanArchitecture.WebUI.Services;

public class IdempotentRequest : IIdempotentRequest
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdempotentRequest(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? IdempotencyKey => _httpContextAccessor.HttpContext?.Request.Headers[HeaderNames.IdempotencyKey];
}
