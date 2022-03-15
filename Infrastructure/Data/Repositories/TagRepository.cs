using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly HappyKidsContext _context;
        public TagRepository(HappyKidsContext context)
        {
            _context = context;
        }

        public async Task<List<Tag>> GetAllTags(QueryParameters queryParameters)
        {
            IQueryable<Tag> tags = _context.Tags.AsQueryable().OrderBy(x => x.Name);

            if (queryParameters.HasQuery())
            {
                tags = tags.Where(t => t.Name.Contains(queryParameters.Query));
            }

            tags = tags.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);

            return await tags.ToListAsync();
        }

        public async Task<int> GetCountForTags()
        {
            return await _context.Tags.CountAsync();
        }

        public async Task<Tag> GetTagById(int id)
        {
            return await _context.Tags.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CreateTag(Tag tag)
        {
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();                    
        }

        public async Task UpdateTag(Tag tag)
        {
            _context.Entry(tag).State = EntityState.Modified;  
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTag(Tag tag)
        {
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
        }
    }
}