using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Dtos.Identity;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly HappyKidsContext _context;
        public AdminRepository(HappyKidsContext context)
        {
            _context = context;
        }

        public async Task<List<UserToReturnDto>> GetAllUsers(QueryParameters queryParameters)
        {
            IQueryable<UserToReturnDto> users = (from u in _context.Users
                                               select new UserToReturnDto 
                                               {
                                                   Username = u.UserName,
                                                   Email = u.Email,
                                                   UserId = u.Id,
                                                   LockoutEnd = u.LockoutEnd,
                                                   Roles = u.UserRoles.Select(r => r.Role.Name).ToList()

                                                }).AsQueryable().OrderBy(x => x.Username);
            
            if (queryParameters.HasQuery())
            {
                users = users.Where(x => x.Username.Contains(queryParameters.Query));
            }

            users = users.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);
            
            return await users.ToListAsync();       
        }

        public async Task<int> GetCountForUsers()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<ApplicationUser> FindUserById(int id)
        {
            return await _context.Users.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task LockUser(int id)
        {
            var userFromDb = await FindUserById(id);

            userFromDb.LockoutEnd = DateTime.Now.AddYears(1000);

            await _context.SaveChangesAsync();
        }

        public async Task UnlockUser(int id)
        {
            var userFromDb = await FindUserById(id);

            userFromDb.LockoutEnd = null;

            await _context.SaveChangesAsync();
        }
    }
}










