using System.Threading.Tasks;
using Core.Dtos;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IRatingLikeRepository
    {
        Task<bool> CheckIfClientHasOrderedThisChildrenItem(int childrenItemId, string email);
        Task<Rating> FindCurrentRate(int childrenItemId, int userId);
        Task<bool> ChechIfAny(int childrenItemid);
        Task<double> AverageVote(int childrenItemid);
        Task AddRating(RatingDto ratingDto, int userId);
        Task<bool> CheckIfUserHasAlreadyLikedThisProduct(int userId, int childrenItemId);
        Task AddLike(int userId, int childrenItemId);
        Task<int> GetCountForLikes(int childrenItemId);
    }
}



