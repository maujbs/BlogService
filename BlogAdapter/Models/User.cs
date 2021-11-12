using System;

namespace BlogAdapter.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Token { get; set; }
        public int RoleLevel { get; set; }
    }
}
