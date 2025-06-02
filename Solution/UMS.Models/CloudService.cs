using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Models
{
    public enum ServiceType
    {
        SaaS,
        PaaS,
        IaaS
    }

    public class CloudService
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public ServiceType Type { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(255)]
        public string Provider { get; set; }

        [StringLength(255)]
        public string WebsiteUrl { get; set; }

        public decimal? MonthlyCost { get; set; }

        public bool HasFreeVersion { get; set; }

        [StringLength(1000)]
        public string Features { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime? LastUpdated { get; set; }
    }
}
