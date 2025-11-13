namespace Auth.Business.Contracts.Responses;

public record LoginResponse(bool IsValid, string Message);
