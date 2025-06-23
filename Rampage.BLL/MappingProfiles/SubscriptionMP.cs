using AutoMapper;
using Rampage.BLL.DTO_s.SubscriptionDTO_s;
using Rampage.Core.Entities;

namespace Rampage.BLL.MappingProfiles
{
    public class SubscriptionMP : Profile
    {
        public SubscriptionMP()
        {


            CreateMap<CreateSubscriptionDTO, Subscription>()
                .ReverseMap();
        }
    }
}
