using AutoMapper;
using Rampage.BLL.DTO_s.CategoryDTO_s;
using Rampage.BLL.MappingProfiles.Commons;
using Rampage.Core.Entities;

namespace Rampage.BLL.MappingProfiles
{
    public class CategoryMP : Profile
    {
        public CategoryMP()
        {


            CreateMap<CreateCategoryDTO, Category>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .AfterMap<CustomMappingAction<CreateCategoryDTO, Category>>()
                .ReverseMap();
            CreateMap<CreateCategoryTranslationDTO, CategoryTranslation>()
                .ReverseMap();



            CreateMap<UpdateCategoryDTO, Category>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .AfterMap<CustomMappingAction<UpdateCategoryDTO, Category>>()
                .ReverseMap();
            CreateMap<UpdateCategoryTranslationDTO, CategoryTranslation>()
                .ReverseMap();
        }
    }
}
