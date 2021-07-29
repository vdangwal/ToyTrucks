using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.Api.DBContexts;
using Basket.Api.Dtos;
using MongoDB.Driver;

namespace Basket.Api.Services
{
    public class BasketLinesService : IBasketLinesService
    {
        private readonly IBasketContext _context;
        public BasketLinesService(IBasketContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<BasketLine> AddOrUpdateBasketLine(Guid basketId, BasketLine basketLine)
        {
            throw new NotImplementedException();
        }

        public Task<BasketLine> GetBasketLineById(Guid basketLineId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BasketLine>> GetBasketLinesById(Guid basketId)
        {
            if (basketId == Guid.Empty)
            {
                return null;
            }

            FilterDefinition<BasketLine> filter = Builders<BasketLine>.Filter.Eq(bl => bl.BasketId, basketId);
            var basketLines = await _context.CartLines
                                 .Find(filter)
                                 .ToListAsync();
            //.FirstOrDefaultAsync();

            return basketLines != null ? basketLines : new List<BasketLine>();
        }

        public void RemoveBasketLine(BasketLine basketLine)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void UpdateBasketLine(BasketLine basketLine)
        {
            throw new NotImplementedException();
        }
    }
}