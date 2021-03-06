using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("reservation")]
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public decimal Quantity { get; set; }
        public short Pin { get; set; }
        [Required]
        public virtual User User { get; set; }
        [Required]
        public virtual Offer Offer { get; set; }
    }
}