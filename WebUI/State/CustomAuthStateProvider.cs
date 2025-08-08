﻿using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebUI.State
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private const string LocalStorageKey = "auth";

        private readonly ClaimsPrincipal anonymous = new(new ClaimsIdentity());
        private readonly ILocalStorageService localStorageService;

        public CustomAuthStateProvider(ILocalStorageService localStorageService)
        {
            this.localStorageService = localStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string token = await localStorageService.GetItemAsStringAsync(LocalStorageKey);
            if (string.IsNullOrEmpty(token))
                return await Task.FromResult(new AuthenticationState(anonymous));

            var (name, email, role) = GetClaims(token);
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(name))
                return await Task.FromResult(new AuthenticationState(anonymous));

            var claims = SetClaimPrincipal(name, email, role);
            if (claims is null)
                return await Task.FromResult(new AuthenticationState(anonymous));
            else
                return await Task.FromResult(new AuthenticationState(claims));
        }

        public static ClaimsPrincipal SetClaimPrincipal(string name, string email, string role)
        {
            if (name is null || email is null)
                return new ClaimsPrincipal();

            return new ClaimsPrincipal(new ClaimsIdentity(
              [
                    new(ClaimTypes.Name, name!),
                    new(ClaimTypes.Email, email!),
                    new(ClaimTypes.Role, role!)
                ], "JwtAuth"));
        }

        private static (string, string, string) GetClaims(string jwtToken)
        {
            if (string.IsNullOrEmpty(jwtToken)) return (null!, null!, null!);

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);

            var name = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Name)!.Value;
            var email = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Email)!.Value;
            var role = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Role)!.Value;
            return (name, email, role);
        }

        public async Task UpdateAuthState(string jwtToken)
        {
            var claims = new ClaimsPrincipal();

            if (!string.IsNullOrEmpty(jwtToken))
            {
                var (name, email, role) = GetClaims(jwtToken);
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
                    return;

                var setClaims = SetClaimPrincipal(name, email, role);
                if (setClaims is null) return;

                await localStorageService.SetItemAsStringAsync(LocalStorageKey, jwtToken);
            }
            else
            {
                await localStorageService.RemoveItemAsync(LocalStorageKey);
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claims)));
        }
    }
}
