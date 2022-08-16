using System.Security.Claims;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class UserManagerExtensions
    {
        /// <summary>
        /// We use this static method to retrieve user data including address
        /// See AccountController/GetClientAddress for more details
        /// </summary>
        public static async Task<ApplicationUser> FindUserWithAddressByClaims(
                this UserManager<ApplicationUser> input, ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);
            
            return await input.Users.Include(x => x.Address).SingleOrDefaultAsync(x => x.Email == email);
        }
    }
}