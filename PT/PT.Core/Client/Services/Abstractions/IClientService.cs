using PT.Core.Client.DTO;
using System.Threading.Tasks;

namespace PT.Core.Client.Services.Abstractions
{
    public interface IClientService
    {
        Task<ClientDTO> CreateAsync(ClientDTO model);
        ClientDTO Get(int id);
    }
}
