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
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        
        public Address Address { get; set; }
        public List<Offer> Offers { get; set; }
    }
}