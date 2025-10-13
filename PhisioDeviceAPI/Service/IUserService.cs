using PhisioDeviceAPI.DTOs.User;
using PhisioDeviceAPI.Models;

namespace PhisioDeviceAPI.Service
{
    public interface IUserService
    {
        Task<User> RegisterAsync(UserDTO dto, CancellationToken cancellationToken = default);
        Task<string> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
    }
}


