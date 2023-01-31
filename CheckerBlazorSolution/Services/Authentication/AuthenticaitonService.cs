using System;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;


namespace CheckerBlazorServer.Services.Authentication;

//public class NewAuthenticationStateProvider : AuthenticationStateProvider
//{
//    public override Task<AuthenticationState> GetAuthenticationStateAsync()
//    {
//        var identity = new ClaimsIdentity(new[]
//        {
//            new Claim(ClaimTypes.Name, "mrfibuli"),

//            new Claim(ClaimTypes.Name, identityUser.Email),
//            new Claim(ClaimTypes.Email, identityUser.Email),

//        }, "Fake authentication type");




//        var user = new ClaimsPrincipal(identity);

//        return Task.FromResult(new AuthenticationState(user));
//    }


/*


    var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, identityUser.Email),
            new Claim(ClaimTypes.Email, identityUser.Email),
        };
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    IList<string> roleNames = await _userManager.GetRolesAsync(identityUser);
    claims.AddRange(roleNames.Select(roleName => new Claim(ClaimsIdentity.DefaultRoleClaimType, roleName)));


    var expiration = DateTime.UtcNow.AddYears(1);
    JwtSecurityToken token = new JwtSecurityToken(
        issuer: null,
        audience: null,
        claims: claims,
        expires: expiration,
        signingCredentials: creds
        );

    return new UserToken()
    {
        Token = new JwtSecurityTokenHandler().WriteToken(token)
    };


 */

//}


public class AuthenticationService
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;

    public AuthenticationService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
    }

    public async Task Login(string email, string password)
    {
        var emailUser = await userManager.FindByEmailAsync(email);
        var correctlogin = await userManager.CheckPasswordAsync(emailUser, password);



        await signInManager.SignInAsync(emailUser, true);
    }
}