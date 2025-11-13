using Auth.Business.Contracts.Requests;
using Auth.Business.Contracts.Responses;

namespace Auth.Business.Services;

public interface IAuthService
{
    Task<LoginResponse> ValidateCredentialsAsync(LoginRequest request, CancellationToken cancellationToken = default);
}
