using Microsoft.AspNetCore.Http;
using PT.Identity.Abstractions;

namespace PT.Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor accessor;

        public IdentityService(IHttpContextAccessor accessor)
        {
            this.accessor = accessor;
        }
        public IUser User { get { return new User(this.accessor.HttpContext.User); } }
    }
}
