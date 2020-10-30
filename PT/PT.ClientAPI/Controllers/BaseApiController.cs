using Microsoft.AspNetCore.Mvc;
using PT.Identity.Abstractions;

namespace PT.ClientAPI.Controllers
{
    public abstract class BaseApiController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        public BaseApiController(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        protected IUser UserInfo => _identityService.User;
    }
}
