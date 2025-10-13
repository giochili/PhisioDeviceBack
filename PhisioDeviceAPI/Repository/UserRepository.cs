using Microsoft.EntityFrameworkCore;
using PhisioDeviceAPI.Data;
using PhisioDeviceAPI.Models;

namespace PhisioDeviceAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
        {
            await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return user;
        }

        public async Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }
    }
}


