using AutoMapper;
using Rampage.BLL.DTO_s.SettingDTO_s;
using Rampage.BLL.MappingProfiles.Commons;
using Rampage.Core.Entities;

namespace Rampage.BLL.MappingProfiles
{
    public class SettingMP : Profile
    {
        public SettingMP()
        {


            CreateMap<SettingDTO, Setting>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .AfterMap<CustomMappingAction<SettingDTO, Setting>>()
                .ReverseMap();
        }
    }
}
