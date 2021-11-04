using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table(("pickup"))]
    public class Pickup
    {
        public int Id { get; set; }
        [Required]
        public Reservation Reservation { get; set; }
        [Required]
        public bool UserConfirmed { get; set; }
        [Required]
        public bool GiverConfirmed { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}