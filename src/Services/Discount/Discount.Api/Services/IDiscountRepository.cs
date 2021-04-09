using Discount.API.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace Discount.API.Services
{
    public interface IDiscountRepository
    {
        Task<IEnumerable<Coupon>> GetCoupons();
        Task<Coupon> GetDiscount(string productName);
        Task<bool> CreateDiscount(Coupon coupon);
        Task<bool> UpdateDiscount(Coupon coupon);
        Task<bool> DeleteDiscount(string productName);
    }
}
