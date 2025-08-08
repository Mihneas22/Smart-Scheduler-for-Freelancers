using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Service
    {
        [Key]
        public Guid Id { get; set; }

        public User? Freelancer { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public int? DurationsMin { get; set; }

        public float? Price { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
