using IlmPath.Application.Common.Interfaces;
using IlmPath.Domain.Entities;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IlmPath.Infrastructure.Carts
{
    public class RedisCartRepository:ICartRepository
    {
        private readonly StackExchange.Redis.IDatabase _database;

        public RedisCartRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        private string GetKey(string userId) => $"cart:{userId}";


        public async Task<bool> DeleteCartAsync(string userId)
        {
            return await _database.KeyDeleteAsync(GetKey(userId));
        }


        public async Task<Cart?> GetCartAsync(string userId)
        {
            var data = await _database.StringGetAsync(GetKey(userId));

            if (data.IsNullOrEmpty)
            {
                return null;
            }

            return JsonSerializer.Deserialize<Cart>(data!);
        }


        public async Task<Cart> UpdateCartAsync(Cart cart)
        {
            var serializedCart = JsonSerializer.Serialize(cart);

            await _database.StringSetAsync(GetKey(cart.UserId), serializedCart, TimeSpan.FromDays(7));

            return cart;
        }

    }
}
