using Blessings.Contract.Settings;
using Blessings.Contract.TokenValidation;
using Microsoft.AspNetCore.Http;

namespace Blessings.Contract;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AppSetting _appSetting;

    public JwtMiddleware(RequestDelegate next, AppSetting appSetting)
    {
        _next = next;
        _appSetting = appSetting;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        var user = TokenValidator.ValidateToken(token, _appSetting);

        if (user == null || (user.UserId==null && user.Role==null))
        {
            throw new UnauthorizedAccessException("Invalid token");
        }

        context.Items["Role"] = user.Role;

        await _next(context);
    }
}