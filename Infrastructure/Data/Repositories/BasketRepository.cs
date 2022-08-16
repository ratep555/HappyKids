using System;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities.ClientBaskets;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Data.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        // redis database
        private readonly IDatabase _db;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        /// <summary>
        /// Deletes client basket
        /// </summary>
        public async Task<bool> DeleteClientBasket(string basketId)
        {
            return await _db.KeyDeleteAsync(basketId);
        }

        /// <summary>
        /// Updates client basket
        /// We will keep the basket for 30 days
        /// </summary>
        public async Task<ClientBasket> UpdateClientBasket(ClientBasket basket)
        {
            var updated = await _db.StringSetAsync(basket.Id, 
                JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));

            if (!updated) return null;

            return await GetClientBasket(basket.Id);       
        }

        /// <summary>
        /// Gets the corresponding basket (which is stored as string in redis db) based on basketId
        /// </summary>
        public async Task<ClientBasket> GetClientBasket(string basketId)
        {
            var basket = await _db.StringGetAsync(basketId);

            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<ClientBasket>(basket);
        }
    }
}