using Auth.Business.Entities;

namespace Auth.Business.Repositories;

public interface IUserRepository
{
    Task<User?> FindByUsernameAsync(string username, CancellationToken cancellationToken = default);
}
