using System;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class UserRegistration
    {
        public string Name { get; set; } = string.Empty;

        public string Surname { get; set; } = string.Empty;

        [EmailAddress()]
        public string Email { get; set; } = string.Empty;

        [MinLength(8)]
        public string Password { get; set; } = string.Empty;
    }
    public class UserLogin
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class UserReturn
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public UserReturn(int Id, string Name, string Surname, string Email, string Token)
        {
            this.Id = Id;
            this.Name = Name;
            this.Surname = Surname;
            this.Email = Email;
            this.Token = Token;
        }

        public UserReturn(int Id, string Name, string Surname, string Email){
            this.Id = Id;
            this.Name = Name;
            this.Surname = Surname;
            this.Email = Email;
        }

        public string? Token { get; set; } 
    }
}