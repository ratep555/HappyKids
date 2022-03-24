using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IBranchRepository
    {
        Task<List<Branch>> GetAllBranches(QueryParameters queryParameters);
        Task<int> GetCountForBranches();
        Task<List<Branch>> GetLocations();
        Task<Branch> GetBranchById(int id);
        Task CreateBranch(Branch branch);
        Task UpdateBranch(Branch branch);
        Task DeleteBranch(Branch branch);
    }
}