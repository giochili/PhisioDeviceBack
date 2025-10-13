using PhisioDeviceAPI.Models;

namespace PhisioDeviceAPI.Repository
{
    public interface IUserRepository
    {
       Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
        Task<User> AddAsync(User user, CancellationToken cancellationToken = default);
        Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}