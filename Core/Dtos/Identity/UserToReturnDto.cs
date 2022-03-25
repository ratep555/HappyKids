using System;
using System.Collections.Generic;

namespace Core.Dtos.Identity
{
    public class UserToReturnDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public List<string> Roles { get; set; }
    }
}