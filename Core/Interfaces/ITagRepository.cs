using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface ITagRepository
    {
        Task<List<Tag>> GetAllTags(QueryParameters queryParameters);
        Task<int> GetCountForTags();
        Task<Tag> GetTagById(int id);
        Task CreateTag(Tag tag);
        Task UpdateTag(Tag tag);
        Task DeleteTag(Tag tag);
    }
}