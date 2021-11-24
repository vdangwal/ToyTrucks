// using System.Collections.Generic;
// using AutoMapper;
// using Discount.Grpc.Converters;
// using Google.Protobuf.Collections;
// using protos = Discount.Grpc.Protos;

// namespace Discount.Grpc.Profiles
// {
//     public class CouponProfile : Profile
//     {
//         public CouponProfile()
//         {
//             CreateMap<Dtos.Coupon, Models.Coupon>();
//             CreateMap<Models.Coupon, Dtos.Coupon>().ReverseMap();

//             CreateMap<Dtos.Coupon, protos.Coupon>().ReverseMap();
//             CreateMap(typeof(IEnumerable<>), typeof(RepeatedField<>)).ConvertUsing(typeof(EnumerableToRepeatedFieldTypeConverter<,>));
//         }
//     }
// }