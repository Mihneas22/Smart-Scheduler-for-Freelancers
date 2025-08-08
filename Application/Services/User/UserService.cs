using Application.DTOs.User.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.User
{
    public class UserService : IUserService
    {
        private readonly HttpClient httpClient;
        public UserService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<LoginUserResponse> LoginUserService(LoginUserDTO loginUserDTO)
        {
            var request = await httpClient.PostAsJsonAsync("api/user/login", loginUserDTO);
            var result = await request.Content.ReadFromJsonAsync<LoginUserResponse>();
            return result!;
        }

        public async Task<RegisterUserResponse> RegisterUserService(RegisterUserDTO registerUserDTO)
        {
            var request = await httpClient.PostAsJsonAsync("api/user/register", registerUserDTO);
            var result = await request.Content.ReadFromJsonAsync<RegisterUserResponse>();
            return result!;
        }
    }
}
