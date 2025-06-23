using AutoMapper;
using Rampage.BLL.DTO_s.AuthenticationDTO_s;
using Rampage.Core.Entities.Identity;

namespace Rampage.BLL.MappingProfiles
{
    public class UserMP : Profile
    {
        public UserMP()
        {
            // Create Subscription

            CreateMap<RegisterDTO, User>()
                .ReverseMap();
        }
    }
}
