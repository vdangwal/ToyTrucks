using System.Collections.Generic;
using System.Threading.Tasks;
using Basket.Api.Dtos;

namespace Basket.Api.Services
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(string userId);
        IEnumerable<string> GetUsers();
        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
        Task<bool> DeleteBasketAsync(string id);
    }
}