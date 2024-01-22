namespace InterviewCrud.Api.Identity.InputModels;

public record RequestLogin
{
    public string? EmailOrUserName { get; set; }
    public string? Password { get; set; }
}

public class RequestRegister
{
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Base64Profile { get; set; }
    public string? ProfilePhoto { get; set; }
}