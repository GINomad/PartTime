using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PT.Core.Client.DTO;
using PT.Core.Client.Services.Abstractions;
using PT.Identity.Abstractions;
using System.Threading.Tasks;

namespace PT.ClientAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientController : BaseApiController
    {
        private readonly IClientService _clientService;
        private readonly IApplicationUserService _appUserService;

        public ClientController(IIdentityService identity, IClientService clientService, IApplicationUserService appUserService): base(identity)
        {
            _clientService = clientService;
            _appUserService = appUserService;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var client = _clientService.Get(id);

            return Ok(client);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClientDTO client)
        {
            var result = await _clientService.CreateAsync(client);
            await _appUserService.AssignClientAsync(UserInfo.Id, result.Id);

            return Ok(result);
        }
    }
}
