using PT.Identity.Abstractions;
using System.Linq;
using System.Security.Claims;

namespace PT.Identity
{
    public class User : IUser
    {
        private readonly ClaimsPrincipal _user;
        public User(ClaimsPrincipal user)
        {
            _user = user;
        }
        public string Id { get { return _user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value; } }

        public int? ClientId { 
            get 
            {   var claim = _user.Claims.FirstOrDefault(c => c.Type == "client_user_id")?.Value;
                return claim != null ? (int?)int.Parse(claim) : null; 
            } 
        }

    }
}
