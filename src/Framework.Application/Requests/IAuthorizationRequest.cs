namespace Framework.Application.Requests;

public interface IAuthorizationRequest
{
    IEnumerable<string> RequiredPolicies { get; }
    IEnumerable<string> RequiredRoles { get; }
}
