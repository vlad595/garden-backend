using System;

namespace Models
{
    public class User : BaseEntity
    {
        public string Name {get;set;}
        public string Surname {get;set;}
        public string Email {get;set;}
        public string PasswordHash {get;set;}
        public UsersRoles Role {get;set;} = 0;

        public List<Plant> Plants {get;set;} = new List<Plant>();
    }

    public enum UsersRoles
    {
        User,
        Moderator,
        Admin
    }
}