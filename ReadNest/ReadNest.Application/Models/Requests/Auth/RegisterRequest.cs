﻿namespace ReadNest.Application.Models.Requests.Auth
{
    public class RegisterRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
