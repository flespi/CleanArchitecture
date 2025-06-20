using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Types;
using Microsoft.Net.Http.Headers;

namespace CleanArchitecture.Web.Services;

public class ConditionalParameters : IConditionalParameters
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ConditionalParameters(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Hex? IfMatch => _httpContextAccessor.HttpContext?.Request.Headers[HeaderNames.IfMatch].FirstOrDefault();
}
