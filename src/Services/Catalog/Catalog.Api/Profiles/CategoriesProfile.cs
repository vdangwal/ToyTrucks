using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Api.Entities;

namespace Catalog.Api.Profiles
{
    public class CategoriesProfile : Profile
    {
        public CategoriesProfile()
        {

            CreateMap<Models.CategoryDto, Category>()
               .ReverseMap();

        }
    }
}
