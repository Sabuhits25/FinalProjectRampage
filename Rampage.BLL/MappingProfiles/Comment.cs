using AutoMapper;
using Rampage.BLL.DTO_s.CommentDTO_s;
using Rampage.BLL.MappingProfiles.Commons;
using Rampage.Core.Entities;

namespace Rampage.BLL.MappingProfiles
{
    public class CommentMP : Profile
    {
        public CommentMP()
        {


            CreateMap<CreateCommentDTO, Comment>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .AfterMap<CustomMappingAction<CreateCommentDTO, Comment>>()
                .ReverseMap();



            CreateMap<UpdateCommentDTO, Comment>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .AfterMap<CustomMappingAction<UpdateCommentDTO, Comment>>()
                .ReverseMap();
        }
    }
}
