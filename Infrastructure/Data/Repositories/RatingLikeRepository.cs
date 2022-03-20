using System.Linq;
using System.Threading.Tasks;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class RatingLikeRepository : IRatingLikeRepository
    {
        private readonly HappyKidsContext _context;
        public RatingLikeRepository(HappyKidsContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckIfClientHasOrderedThisChildrenItem(int childrenItemId, string email)
        {
            var orderChildrenItem = await _context.OrderChildrenItems
                .FirstOrDefaultAsync(x => x.BasketChildrenItemOrdered.BasketChildrenItemOrderedId == childrenItemId);
            
            var orders = await _context.ClientOrders.Include(x => x.OrderChildrenItems)
                .Where(x => x.CustomerEmail == email &&
                 x. OrderChildrenItems.Contains(orderChildrenItem)).ToListAsync();

           if (!orders.Any())
           {
               return true;
           }
           return false;
        }

        public async Task<Rating> FindCurrentRate(int childrenItemId, int userId)
        {
                return await _context.Ratings.Include(x => x.Client)
                .Where(x => x.ChildrenItemId == childrenItemId && x.ApplicationUserId == userId)
                .FirstOrDefaultAsync();        
        }

        public async Task<bool> ChechIfAny(int childrenItemid)
        {
           return await _context.Ratings.AnyAsync(x => x.ChildrenItemId == childrenItemid);          
        }

        public async Task<double> AverageVote(int childrenItemid)
        {
            var average = await _context.Ratings.Where(x => x.ChildrenItemId == childrenItemid).AverageAsync(x => x.Rate);

            return average;
        }

        public async Task AddRating(RatingDto ratingDto, int userId)
        {
            var user = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();

            var rating = new Rating();
            rating.ChildrenItemId = ratingDto.ChildrenItemId;
            rating.Rate = ratingDto.Rating;
            rating.ApplicationUserId = user.Id;

            _context.Ratings.Add(rating);
        }

        public async Task<bool> CheckIfUserHasAlreadyLikedThisProduct(int userId, int childrenItemId)
        {
            var like = await _context.Likes.
                FirstOrDefaultAsync(x => x.ApplicationUserId == userId && x.ChildrenItemId == childrenItemId);
            
            if(like!= null) return true;

            return false;  
        }

        public async Task AddLike(int userId, int childrenItemId)
        {
            var like = new Like()
            {
                ApplicationUserId = userId,
                ChildrenItemId = childrenItemId
            };

            _context.Likes.Add(like);

            await _context.SaveChangesAsync();
        }

        public async Task<int> GetCountForLikes(int childrenItemId)
        {
            return await _context.Likes.Where(x => x.ChildrenItemId == childrenItemId).CountAsync();
        }
    }
}






