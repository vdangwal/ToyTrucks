using ToyTrucks.Discount.Grpc.Dtos;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace ToyTrucks.Discount.Grpc.Services
{
    public interface IDiscountRepository
    {
        Task<bool> DiscountExists(string productName);
        Task<IEnumerable<Coupon>> GetDiscounts();
        Task<Coupon> GetDiscount(string productName);
        Task<Coupon> GetDiscountById(int recordId);
        Task<Coupon> CreateDiscount(Coupon coupon);
        Task<bool> UpdateDiscount(Coupon coupon);
        Task<bool> DeleteDiscount(string productName);
    }
}
