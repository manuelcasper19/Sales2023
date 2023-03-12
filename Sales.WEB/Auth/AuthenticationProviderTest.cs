using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Sales.WEB.Auth
{
    public class AuthenticationProviderTest : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
          
            var stivenUser = new ClaimsIdentity(new List<Claim>
        {
            new Claim("FirstName", "Stiven"),
            new Claim("LastName", "Hernandez"),
            new Claim(ClaimTypes.Name, "stiven@yopmail.com"),
            new Claim(ClaimTypes.Role, "Admin")
        },
            authenticationType: "test");
            var anonimous = new ClaimsIdentity();
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(stivenUser)));
        }
    }

}
