using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Core.Interfaces
{
    public interface IFileStorageService
    {
        Task DeleteFile(string fileRoute, string containerName);
        Task<string> SaveFile(string containerName, IFormFile file);
        Task<string> EditFile(string containerName, IFormFile file, string fileRoute);
 
    }
}