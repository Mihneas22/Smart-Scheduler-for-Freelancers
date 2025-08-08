using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        public string? Username { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? Role { get; set; }

        public ICollection<Service>? Services { get; set; }

        public ICollection<Appointment>? Appointments { get; set; }

        public int? RevenueGeneratedThisMonth { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
