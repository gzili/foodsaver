using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table(("reservation"))]
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public Offer Offer { get; set; }
        [Required]
        public decimal Quantity { get; set; }
    }
}