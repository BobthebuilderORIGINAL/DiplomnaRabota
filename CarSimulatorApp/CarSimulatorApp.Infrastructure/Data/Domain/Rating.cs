using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarSimulatorApp.Infrastructure.Data.Domain
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;

        [Range(1, 5)]
        public int Stars { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
