using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.User.Functions.GetUserData
{
    public class GetUserDTO
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
    }
}
