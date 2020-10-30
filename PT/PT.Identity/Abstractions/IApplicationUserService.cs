using System.Threading.Tasks;

namespace PT.Identity.Abstractions
{
    public interface IApplicationUserService
    {
        Task AssignClientAsync(string userId, int clientId);
    }
}
