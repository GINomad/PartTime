using AutoMapper;
using PT.Core.Client.Domain;
using PT.Core.Client.DTO;

namespace PT.Infrastructure.Mappings
{
    public class ClientMapping: Profile
    {
        public ClientMapping()
        {
            CreateMap<Client, ClientDTO>().ReverseMap();
        }
    }
}
