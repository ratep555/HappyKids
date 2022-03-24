using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class BranchRepository : IBranchRepository
    {
        private readonly HappyKidsContext _context;
        public BranchRepository(HappyKidsContext context)
        {
            _context = context;
        }

        public async Task<List<Branch>> GetAllBranches(QueryParameters queryParameters)
        {
            IQueryable<Branch> barnches = _context.Branches.Include(x => x.Country)
                .AsQueryable().OrderBy(x => x.City);

            if (queryParameters.HasQuery())
            {
                barnches = barnches.Where(t => t.City.Contains(queryParameters.Query));
            }

            barnches = barnches.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);

            return await barnches.ToListAsync();
        }

        public async Task<List<Branch>> GetLocations()
        {
            return await _context.Branches.Include(X => X.Country)
                .OrderBy(x => x.City).ToListAsync();
        }

        public async Task<int> GetCountForBranches()
        {
            return await _context.Branches.CountAsync();
        }

        public async Task<Branch> GetBranchById(int id)
        {
            return await _context.Branches.Include(x => x.Country).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateBranch(Branch branch)
        {
            _context.Branches.Add(branch);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateBranch(Branch branch)
        {
            _context.Entry(branch).State = EntityState.Modified;     

            await _context.SaveChangesAsync();
        }

        public async Task DeleteBranch(Branch branch)
        {
            _context.Branches.Remove(branch);

            await _context.SaveChangesAsync();
        }
    }
}












