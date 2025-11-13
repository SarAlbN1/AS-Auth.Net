using Auth.Business.Entities;
using Auth.Business.Repositories;
using Auth.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Auth.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AuthDbContext _dbContext;

    public UserRepository(AuthDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<User?> FindByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Username == username, cancellationToken);
    }
}
