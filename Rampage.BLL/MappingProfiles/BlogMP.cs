using AutoMapper;
using Rampage.BLL.DTO_s.BlogDTO_s;
using Rampage.BLL.MappingProfiles.Commons;
using Rampage.Core.Entities;

namespace Rampage.BLL.MappingProfiles
{
    public class BlogMP : Profile
    {
        public BlogMP()
        {


            CreateMap<CreateBlogDTO, Blog>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .AfterMap<CustomMappingAction<CreateBlogDTO, Blog>>()
                .ReverseMap();
            CreateMap<CreateBlogTranslationDTO, BlogTranslation>()
                .ReverseMap();



            CreateMap<UpdateBlogDTO, Blog>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .AfterMap<CustomMappingAction<UpdateBlogDTO, Blog>>()
                .ReverseMap();
            CreateMap<UpdateBlogTranslationDTO, BlogTranslation>()
                .ReverseMap();
        }
    }
}
