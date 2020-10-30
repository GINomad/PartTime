using PT.BuildingBlocks.Abstractions;
using PT.Core.Client.DTO;
using PT.Core.Client.Services.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace PT.Core.Client.Services
{
    public class ClientService : IClientService
    {
        private readonly IRepository<Domain.Client> _clientRepository;
        private readonly IEntityConverter _converter;
        public ClientService(IEntityConverter converter, IRepository<Domain.Client> clientRepository)
        {
            _converter = converter;
            _clientRepository = clientRepository;
        }

        public async Task<ClientDTO> CreateAsync(ClientDTO model)
        {
            var domain = _converter.ConvertTo<ClientDTO, Domain.Client>(model);
            await _clientRepository.CreateAsync(domain);
            await _clientRepository.SaveChangesAsync();
            model.Id = domain.Id;

            return model;          
        }

        public ClientDTO Get(int id)
        {
            var client = _clientRepository.Get(c => c.Id == id).FirstOrDefault();

            return _converter.ConvertTo<Domain.Client, ClientDTO>(client);
        }
    }
}
