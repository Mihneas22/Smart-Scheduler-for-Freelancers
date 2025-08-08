using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Appointment
    {
        [Key]
        public Guid Id { get; set; }

        public string? ClientName { get; set; }

        public ICollection<User>? FreelancerUsers { get; set; }

        public string? Title { get; set; }

        public List<string>? Notes { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string? Status { get; set; }

        public bool? ReminderSent { get; set; }
    }
}
