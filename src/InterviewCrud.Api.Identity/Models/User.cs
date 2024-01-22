using Microsoft.AspNetCore.Identity;

namespace InterviewCrud.Api.Identity.Models;

public class User : IdentityUser
{
    public string? ProfileImage { get; set; }
}