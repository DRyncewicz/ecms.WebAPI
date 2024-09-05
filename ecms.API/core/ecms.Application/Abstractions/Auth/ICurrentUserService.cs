namespace ecms.Application.Abstractions.Auth;

/// <summary>
/// Service to provide claims and authorization information about the user associated with the request
/// TODO: Implement User class, add claim retrieval and other identity
/// </summary>
public interface ICurrentUserService
{
    string UserId { get; set; }

    bool IsAuthenticated { get; set; }
}