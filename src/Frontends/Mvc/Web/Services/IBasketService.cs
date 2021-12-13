using System.Threading.Tasks;
using Web.Models.Api;

namespace Web.Services
{
    public interface IBasketService
    {
        Task<CustomerBasket> GetBasket(string userId);
        Task<CustomerBasket> AddLine(string basketId, BasketItem basketItem);
        Task DeleteBasket(string userId);
        Task UpdateLine(string basketId, BasketLineForUpdate basketLineForUpdate);
        Task<CustomerBasket> RemoveLine(string basketId, string lineId);
        Task<BasketForCheckout> Checkout(string basketId, BasketForCheckout basketForCheckout);
    }
}