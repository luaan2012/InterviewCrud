using InterviewCrud.Api.Identity.InputModels;
using InterviewCrud.Api.Identity.Services;

namespace InterviewCrud.Api.Identity.EndPoints;

public static class UserEndpoint
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapPost("signup", SignUp);
        app.MapPost("signin", SignIn);
    }
    
    public static async Task<IResult> SignUp(RequestRegister request, IAuthServices authServices)
    {
        var result = await authServices.SignUp(request);

        return result.Match<IResult>(
            m => Results.Ok(m),
            err => Results.BadRequest(err)
        );
    }
    
    public static async Task<IResult> SignIn(RequestLogin request, IAuthServices authServices)
    {
        var result = await authServices.SignIn(request);

        return result.Match<IResult>(
            m => Results.Ok(m),
            err => Results.BadRequest(err)
        );
    }   
}