using AutoMapper;
using Rampage.BLL.DTO_s.ContactDTO_s;
using Rampage.Core.Entities;

namespace Rampage.BLL.MappingProfiles
{
    public class ContactMP : Profile
    {
        public ContactMP()
        {
            // Create Contact

            CreateMap<CreateContactDTO, Contact>()
                .ReverseMap();
        }
    }
}
