using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.User.Auth
{
    public record LoginUserResponse(bool Flag, string message = null!, string token = null!);
}
