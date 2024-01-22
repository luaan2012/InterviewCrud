namespace InterviewCrud.Api.Identity.OutputModels;

public class UserLoginResponse(string at, string rf, string userId, int expires, string profileImage, string email, string name)
{
    public string AccessToken { get; set; } = at;
    public string RefreshToken { get; set; } = rf;
    public string UserId { get; set; } = userId;
    public string ProfileImage { get; set; } = profileImage;
    public string Email { get; set; } = email;
    public string Name { get; set; } = name;
    public int ExpiresIn { get; set; } = expires;
}