using System.Collections.Generic;
using AutoMapper;
using Google.Protobuf.Collections;

namespace ToyTrucks.Discount.Grpc.Converters
{
    public class EnumerableToRepeatedFieldTypeConverter<TItemSource, TItemDest> : ITypeConverter<IEnumerable<TItemSource>, RepeatedField<TItemDest>>
    {
        public RepeatedField<TItemDest> Convert(IEnumerable<TItemSource> source, RepeatedField<TItemDest> destination, ResolutionContext context)
        {
            destination = destination ?? new RepeatedField<TItemDest>();
            foreach (var item in source)
            {
                destination.Add(context.Mapper.Map<TItemDest>(item));
            }
            return destination;
        }
    }
}