using PT.BuildingBlocks.Abstractions;
using PT.Identity.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace PT.Identity.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IRepository<ApplicationUser> _userRepository;

        public ApplicationUserService(IRepository<ApplicationUser> userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task AssignClientAsync(string userId, int clientId)
        {
            var user = _userRepository.Get(u => u.Id == userId).FirstOrDefault();
            user.ClientId = clientId;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
        }
    }
}
