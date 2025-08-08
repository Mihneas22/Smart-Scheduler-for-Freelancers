using Application.DTOs.User.Auth;
using Application.DTOs.User.Functions.GetUserData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repository
{
    public interface IUser
    {
        Task<LoginUserResponse> LoginUserAsync(LoginUserDTO loginUserDTO);

        Task<RegisterUserResponse> RegisterUserAsync(RegisterUserDTO registerUserDTO);
   
        Task<GetUserResponse> GetUserAsync(GetUserDTO userDTO);
    }
}
