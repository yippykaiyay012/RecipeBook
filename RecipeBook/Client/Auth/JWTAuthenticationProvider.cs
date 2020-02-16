using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using RecipeBook.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace RecipeBook.Client.Auth
{
    public class JWTAuthenticationProvider : AuthenticationStateProvider , ILoginService
    {
        private ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;

        private static readonly string _tokenKey = "TOKENKEY";

        private AuthenticationState _anonymous => 
            new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        public JWTAuthenticationProvider(ILocalStorageService localStorage, HttpClient httpClient)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>(_tokenKey);

            if (string.IsNullOrEmpty(token))
            {
                return _anonymous;
            }
            else
            {
                return BuildAuthenticationState(token);
            }
        }



        public async Task SignIn(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                await _localStorage.SetItemAsync(_tokenKey, token);
                var authState = BuildAuthenticationState(token);
                NotifyAuthenticationStateChanged(Task.FromResult(authState));
            }

        }


        public async Task SignOut()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            await _localStorage.RemoveItemAsync(_tokenKey);
            NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));

        }







        private AuthenticationState BuildAuthenticationState(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt")));
        }


        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

            if (roles != null)
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }



        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }


    }
}

