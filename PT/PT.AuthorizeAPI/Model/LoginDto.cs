﻿using System.ComponentModel.DataAnnotations;

namespace PT.AuthorizeAPI.Model
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }

    }
}
