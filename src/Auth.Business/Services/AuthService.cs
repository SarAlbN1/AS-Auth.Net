using Auth.Business.Contracts.Requests;
using Auth.Business.Contracts.Responses;
using Auth.Business.Repositories;
using Auth.Business.Security;

namespace Auth.Business.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResponse> ValidateCredentialsAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return new LoginResponse(false, "Username and password are required.");
        }

        var user = await _userRepository.FindByUsernameAsync(request.Username, cancellationToken);
        if (user is null || !user.IsActive)
        {
            return new LoginResponse(false, "Invalid credentials.");
        }

        var passwordMatches = _passwordHasher.Verify(request.Password, user.PasswordHash);
        return passwordMatches
            ? new LoginResponse(true, "Authentication succeeded.")
            : new LoginResponse(false, "Invalid credentials.");
    }
}
