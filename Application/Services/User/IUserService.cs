using Application.DTOs.User.Auth;
using Application.DTOs.User.Functions.GetUserData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.User
{
    public interface IUserService
    {
        Task<LoginUserResponse> LoginUserService(LoginUserDTO loginUserDTO);

        Task<RegisterUserResponse> RegisterUserService(RegisterUserDTO registerUserDTO);

        Task<GetUserResponse> GetUserService(GetUserDTO getUserDTO);
    }
}
