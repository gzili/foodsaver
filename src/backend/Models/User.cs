﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("user")]
    public class User
    {
        public int Id { get; set; }
        public UserType UserType { get; set; }
        [Required]
        public string Username { get; set; }
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        
        public virtual Address Address { get; set; }
        public virtual List<Offer> Offers { get; set; }
    }
}