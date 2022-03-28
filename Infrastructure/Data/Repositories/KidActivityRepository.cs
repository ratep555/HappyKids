using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.BirthdayOrders;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class KidActivityRepository : IKidActivityRepository
    {
        private readonly HappyKidsContext _context;
        public KidActivityRepository(HappyKidsContext context)
        {
            _context = context;
        }

        public async Task<List<KidActivity>> GetAllKidActivities(QueryParameters queryParameters)
        {
            IQueryable<KidActivity> kidActivities = _context.KidActivities.AsQueryable().OrderBy(x => x.Name);

            if (queryParameters.HasQuery())
            {
                kidActivities = kidActivities.Where(t => t.Name.Contains(queryParameters.Query));
            }

            kidActivities = kidActivities.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);

            return await kidActivities.ToListAsync();
        }

        public async Task<int> GetCountForKidActivities()
        {
            return await _context.KidActivities.CountAsync();
        }

        public async Task<List<KidActivity>> GetAllPureKidActivities()
        {
            return await _context.KidActivities.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<KidActivity> GetKidActivityById(int id)
        {
            return await _context.KidActivities.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CreateKidActivity(KidActivity kidActivity)
        {
            _context.KidActivities.Add(kidActivity);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateKidActivity(KidActivity kidActivity)
        {    
            _context.Entry(kidActivity).State = EntityState.Modified;  

             await _context.SaveChangesAsync();
        }

        public async Task DeleteKidActivity(KidActivity kidActivity)
        {
            _context.KidActivities.Remove(kidActivity);
            await _context.SaveChangesAsync();
        }
    }
}






