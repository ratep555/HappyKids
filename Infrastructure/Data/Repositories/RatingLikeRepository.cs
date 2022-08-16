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
        /// <summary>
        /// Checks if the client has previously ordered this item
        /// If the aforementioned is true, new rate can be created
        /// See ChildrenItemsController/CreateRate for more details
        /// </summary>
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
        /// <summary>
        /// Gets the corresponding rate based on childreitemid and userid
        /// If there is no rate, new rate gets created, otherwise we will update the existing rate
        /// See ChildrenItemsController/CreateRate for more details
        /// </summary>
        public async Task<Rating> FindCurrentRate(int childrenItemId, int userId)
        {
                return await _context.Ratings.Include(x => x.Client)
                .Where(x => x.ChildrenItemId == childrenItemId && x.ApplicationUserId == userId)
                .FirstOrDefaultAsync();        
        }
        /// <summary>
        /// Checks if there is an existing rate in the db so we could calculate average vote/rate
        /// See ChildrenItemsController/GetItemById for more details
        /// </summary>
        public async Task<bool> ChechIfAny(int childrenItemid)
        {
           return await _context.Ratings.AnyAsync(x => x.ChildrenItemId == childrenItemid);          
        }
        /// <summary>
        /// Checks if there is an existing rate in the db so we could calculate average vote/rate
        /// See ChildrenItemsController/GetItemById for more details
        /// </summary>
        public async Task<double> AverageVote(int childrenItemid)
        {
            var average = await _context.Ratings.Where(x => x.ChildrenItemId == childrenItemid).AverageAsync(x => x.Rate);

            return average;
        }
        /// <summary>
        /// Adds new rate in the case there is no rate
        /// See ChildrenItemsController/CreateRate for more details
        /// </summary>
        public async Task AddRating(RatingDto ratingDto, int userId)
        {
            var user = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();

            var rating = new Rating();
            rating.ChildrenItemId = ratingDto.ChildrenItemId;
            rating.Rate = ratingDto.Rating;
            rating.ApplicationUserId = user.Id;

            _context.Ratings.Add(rating);
        }
        /// <summary>
        /// Checks if the user has already liked this product
        /// See ChildrenItemsController/AddLike for more details
        /// </summary>
        public async Task<bool> CheckIfUserHasAlreadyLikedThisProduct(int userId, int childrenItemId)
        {
            var like = await _context.Likes.
                FirstOrDefaultAsync(x => x.ApplicationUserId == userId && x.ChildrenItemId == childrenItemId);
            
            if(like!= null) return true;

            return false;  
        }
        /// <summary>
        /// Adds like upon authentication scheck
        /// See ChildrenItemsController/AddLike for more details
        /// </summary>
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
        /// <summary>
        /// Counts the number of likes with regards to certain children item
        /// It is initiated upon retrieval of all the children items
        /// See ChildrenItemsController/GetAllChildrenItems for more details
        /// </summary>
        public async Task<int> GetCountForLikes(int childrenItemId)
        {
            return await _context.Likes.Where(x => x.ChildrenItemId == childrenItemId).CountAsync();
        }
    }
}






