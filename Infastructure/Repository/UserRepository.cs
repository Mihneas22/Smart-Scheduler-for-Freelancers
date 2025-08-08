using Application.DTOs.User.Auth;
using Application.Repository;
using Domain.Models;
using Infastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Repository
{
    public class UserRepository : IUser
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IConfiguration configuration;
        public UserRepository(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
        }

        private string GenerateJWTToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            if (user.Role == "Admin")
            {
                var userClaims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, "Admin_User"),
                new Claim(ClaimTypes.Role, "Default_User")
                };

                var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            else
            {
                var userClaims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, "Default_User")
                };

                var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }

        public async Task<LoginUserResponse> LoginUserAsync(LoginUserDTO loginUserDTO)
        {
            if(loginUserDTO == null)
                return new LoginUserResponse(false, "Invalid data");

            var user = await dbContext.UserEntity.FirstOrDefaultAsync(u => u.Email == loginUserDTO.Email);
            if (user == null)
                return new LoginUserResponse(false, "Invalid email or password.");

            bool checkPass = BCrypt.Net.BCrypt.Verify(loginUserDTO.Password, user.Password);
            if (checkPass)
            {
                string token = GenerateJWTToken(user);
                return new LoginUserResponse(true, "Succesfull login!", token);
            }
            else
                return new LoginUserResponse(false, "Invalid email or password.");
        }

        public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserDTO registerUserDTO)
        {
            if (registerUserDTO == null)
                return new RegisterUserResponse(false, "Invalid data.");

            if (registerUserDTO.Password.Length < 8)
                return new RegisterUserResponse(false, "Enter a password with 8 or more characters.");

            if (registerUserDTO.Password != registerUserDTO.ConfirmPassword)
                return new RegisterUserResponse(false, "Passwords do not match.");

            if (registerUserDTO.Email.Contains('@') == false)
                return new RegisterUserResponse(false, "Enter valid email");

            var user = await dbContext.UserEntity.FirstOrDefaultAsync(u => u.Email == registerUserDTO.Email || u.Username == registerUserDTO.Username);
            if (user != null)
                return new RegisterUserResponse(false, "User with email or username already exists.");
                
            dbContext.UserEntity.Add(new User
            {
                Email = registerUserDTO.Email,
                Username = registerUserDTO.Username,
                FullName = registerUserDTO.FullName,
                Password = BCrypt.Net.BCrypt.HashPassword(registerUserDTO.Password),
                Role = registerUserDTO.Role,
                Services = new List<Service>(),
                Appointments = new List<Appointment>(),
                RevenueGeneratedThisMonth = 0,
                CreatedAt = DateTime.Now,
            });

            await dbContext.SaveChangesAsync();

            return new RegisterUserResponse(true, "Success!");
        }
    }
}
