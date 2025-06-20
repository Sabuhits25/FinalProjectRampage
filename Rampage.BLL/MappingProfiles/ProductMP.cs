using AutoMapper;
using Rampage.BLL.DTO_s.ProductDTO_s;
using Rampage.BLL.MappingProfiles.Commons;
using Rampage.Core.Entities;

namespace Rampage.BLL.MappingProfiles
{
    public class ProductMP : Profile
    {
        public ProductMP()
        {
            // Create Product

            CreateMap<CreateProductImageDTO, ProductImage>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .AfterMap<CustomMappingAction<CreateProductImageDTO, ProductImage>>()
                .ReverseMap();

            // Update Product

            CreateMap<UpdateProductImageDTO, ProductImage>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .AfterMap<CustomMappingAction<UpdateProductImageDTO, ProductImage>>()
                .ReverseMap();
        }
    }
}
