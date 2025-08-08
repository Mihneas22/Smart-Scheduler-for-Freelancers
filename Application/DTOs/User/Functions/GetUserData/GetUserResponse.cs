namespace Application.DTOs.User.Functions.GetUserData
{
    public record GetUserResponse(bool Flag, string message = "", Domain.Models.User userData = null!);
}
