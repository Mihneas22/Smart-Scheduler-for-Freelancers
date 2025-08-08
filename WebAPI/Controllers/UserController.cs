using Application.DTOs.User.Auth;
using Application.DTOs.User.Functions.GetUserData;
using Application.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser userRepository;

        public UserController(IUser userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterUserResponse>> RegisterUserAsync(RegisterUserDTO registerUserDTO)
        {
            var result = await userRepository.RegisterUserAsync(registerUserDTO);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginUserResponse>> LoginUserAsync(LoginUserDTO loginUserDTO)
        {
            var result = await userRepository.LoginUserAsync(loginUserDTO);
            return Ok(result);
        }

        [HttpGet("getUser/{username}")]
        public async Task<ActionResult<GetUserResponse>> GetUserAsync(string username)
        {
            var result = await userRepository.GetUserAsync(new GetUserDTO { UserName = username });
            return Ok(result);
        }
    }
}
