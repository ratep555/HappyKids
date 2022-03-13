using System.Threading.Tasks;
using Core.Entities.Identity;

namespace Core.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(ApplicationUser user);
    }
}