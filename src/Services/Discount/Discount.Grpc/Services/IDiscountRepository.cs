using Discount.Grpc.Dtos;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace Discount.Grpc.Services
{
    public interface IDiscountRepository
    {
        Task<bool> DiscountExists(int Id);
        Task<IEnumerable<Coupon>> GetDiscounts();
        Task<Coupon> GetDiscount(string productName);
        Task<Coupon> GetDiscountById(string productId);
        Task<Coupon> CreateDiscount(Coupon coupon);
        Task<bool> UpdateDiscount(Coupon coupon);
        Task<bool> DeleteDiscount(string productId);
    }
}
