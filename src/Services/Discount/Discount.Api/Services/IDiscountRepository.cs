using Discount.API.Dtos;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace Discount.API.Services
{
    public interface IDiscountRepository
    {
        Task<bool> DiscountExists(int Id);
        Task<IEnumerable<Coupon>> GetDiscounts();
        Task<Coupon> GetDiscount(string productName);
        Task<Coupon> GetDiscount(int Id);
        Task<Coupon> CreateDiscount(Coupon coupon);
        Task<bool> UpdateDiscount(Coupon coupon);
        Task<bool> DeleteDiscount(string productName);
    }
}
