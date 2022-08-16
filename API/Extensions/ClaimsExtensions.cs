using System.Security.Claims;

namespace API.Extensions
{
    /// <summary>
    /// Static methods for retrieving username, userid, and email from JWT token
    /// They are later on called within controllers, see for example ChildrenItemsController/CreateRate for more details
    /// </summary>
    public static class ClaimsExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static int GetUserId(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }

        public static string RetrieveEmailFromPrincipal(this ClaimsPrincipal user) 
        {
             return user.FindFirstValue(ClaimTypes.Email);
        }
    }
}