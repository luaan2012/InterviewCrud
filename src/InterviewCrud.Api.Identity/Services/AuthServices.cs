using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using InterviewCrud.Api.Identity.Helper;
using InterviewCrud.Api.Identity.InputModels;
using InterviewCrud.Api.Identity.Models;
using InterviewCrud.Api.Identity.OutputModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Core.Interfaces;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace InterviewCrud.Api.Identity.Services;

public interface IAuthServices
{
    Task<string> GenerateAccessToken(string? email);
    Task<string> GenerateRefreshToken(string? email);
    Task UpdateLastGeneratedClaim(string? email, string jti);
    Task<Result<UserLoginResponse, string>> SignIn(RequestLogin request);
    Task<Result<IdentityResult, IEnumerable<IdentityError>>> SignUp(RequestRegister request);
}

public class AuthServices(
    IJwtService jwtService,
    SignInManager<User> signInManager,
    UserManager<User> userManager,
    IOptionsMonitor<AppSettings> appSettings
) : IAuthServices
{
    private readonly IJwtService _jwtService = jwtService;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly UserManager<User> _userManager = userManager;
    private readonly AppSettings _appSettings = appSettings.CurrentValue;

    public async Task<Result<UserLoginResponse, string>> SignIn(RequestLogin request)
    {
        var result = await _signInManager.PasswordSignInAsync(request.EmailOrUserName, request.Password, false, true);

        if (result.Succeeded)
        {
            var user = await _userManager.FindByNameAsync(request.EmailOrUserName);
            var at = await GenerateAccessToken(user.Email);
            var rt = await GenerateRefreshToken(user.Email);
            return new UserLoginResponse(at, rt, user.Id, _appSettings.Expiration, user.ProfileImage, user.Email, user.UserName);
        }

        return "Não autorizado";
    }

    public async Task<Result<IdentityResult, IEnumerable<IdentityError>>> SignUp(RequestRegister request)
    {
        var user = new User
        {
            UserName = request.UserName,
            Email = request.Email,
            PasswordHash = request.Password,
            PhoneNumber = request.PhoneNumber,
            ProfileImage = request.ProfilePhoto
        };

        if(!string.IsNullOrEmpty(request.ProfilePhoto) || !string.IsNullOrEmpty(request.Base64Profile))
        {
            string fileName = Path.GetFileNameWithoutExtension(request.ProfilePhoto);
            string fileExtension = Path.GetExtension(request.ProfilePhoto);

            string newFileName = fileName + DateTime.Now.ToString("yyyyMMddHHmmss");

            string newProfilePhoto = newFileName + fileExtension;

            request.ProfilePhoto = newProfilePhoto;
            user.ProfileImage = newProfilePhoto;

            var profilePhoto = FileHelper.SaveBase64Image(request.Base64Profile, "Images", request.ProfilePhoto);

            if (!profilePhoto)
            {
                return new List<IdentityError>
                (
                    new IdentityError[] { new IdentityError { Description = "Algo aconteceu ao tentar salvar a imagem." } }
                );
            }
        }

        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {            
            return result;
        }

        FileHelper.DeleteImage("Images", request?.ProfilePhoto);

        return result.Errors.ToArray();
    }

    public async Task<string> GenerateAccessToken(string? email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var userRoles = await _userManager.GetRolesAsync(user);
        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(await _userManager.GetClaimsAsync(user));
        identityClaims.AddClaims(userRoles.Select(s => new Claim("role", s)));

        identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = _appSettings.ValidIssuer, 
            Audience = _appSettings.ValidAudience,
            IssuedAt = DateTime.Now,
            NotBefore = DateTime.Now,
            Expires = DateTime.UtcNow.AddHours(_appSettings.Expiration),
            Subject = identityClaims,
            SigningCredentials = await _jwtService.GetCurrentSigningCredentials()
        });

        return tokenHandler.WriteToken(token);
    }

    public async Task<string> GenerateRefreshToken(string? email)
    {
        var jti = Guid.NewGuid().ToString();
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, jti)
        };

        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);

        var handler = new JwtSecurityTokenHandler();

        var securityToken = handler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = _appSettings.ValidIssuer,              
            Audience = _appSettings.ValidAudience,
            SigningCredentials = await _jwtService.GetCurrentSigningCredentials(),
            Subject = identityClaims,
            NotBefore = DateTime.Now,
            Expires = DateTime.Now.AddDays(30),
            TokenType = "rt+jwt"
        });
        await UpdateLastGeneratedClaim(email, jti);
        var encodedJwt = handler.WriteToken(securityToken);
        return encodedJwt;
    }

    public async Task UpdateLastGeneratedClaim(string? email, string jti)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var claims = await _userManager.GetClaimsAsync(user);
        var newLastRtClaim = new Claim("LastRefreshToken", jti);

        var claimLastRt = claims.FirstOrDefault(f => f.Type == "LastRefreshToken");
        if (claimLastRt != null)
        {
            await _userManager.ReplaceClaimAsync(user, claimLastRt, newLastRtClaim);
        }
        else
        {
            await _userManager.AddClaimAsync(user, newLastRtClaim);
        }
    }
}