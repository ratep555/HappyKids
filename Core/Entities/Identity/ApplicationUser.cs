using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string DisplayName { get; set; }
        public Address Address { get; set; }
        public ICollection<ApplicationUserRole> UserRoles { get; set; }

    }
}