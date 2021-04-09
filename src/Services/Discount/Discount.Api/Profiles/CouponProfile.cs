using AutoMapper;

namespace Discount.API.Profiles
{
    public class CouponProfile : Profile
    {
        public CouponProfile()
        {
            CreateMap<Dtos.Coupon, Models.Coupon>();
            CreateMap<Models.Coupon, Dtos.Coupon>().ReverseMap();
        }
    }
}