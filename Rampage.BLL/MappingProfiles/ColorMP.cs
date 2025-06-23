using AutoMapper;
using Rampage.BLL.DTO_s.ColorDTO_s;
using Rampage.Core.Entities;

namespace Rampage.BLL.MappingProfiles
{
    public class ColorMP : Profile
    {
        public ColorMP()
        {

            CreateMap<CreateColorDTO, Color>()
                .ReverseMap();
            CreateMap<CreateColorTranslationDTO, ColorTranslation>()
                .ReverseMap();



            CreateMap<UpdateColorDTO, Color>()
                .ReverseMap();
            CreateMap<UpdateColorTranslationDTO, ColorTranslation>()
                .ReverseMap();
        }
    }
}
