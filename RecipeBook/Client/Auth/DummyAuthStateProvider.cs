using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RecipeBook.Client.Auth
{
    public class DummyAuthStateProvider : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {

            var authedIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Test User Name")
            }, "test");

            var identity = new ClaimsIdentity();
            await Task.Delay(2000);
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));



            
        }
    }
}
