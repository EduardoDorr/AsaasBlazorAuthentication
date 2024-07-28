using AsaasBlazorAuthentication.Common.ValueObjects;

namespace AsaasBlazorAuthentication.Common.Entities;

public interface ILogin
{
    public Password Password { get; }
    public string Role { get; }
}