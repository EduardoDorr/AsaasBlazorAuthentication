using MediatR;

using AsaasBlazorAuthentication.Common.Results;

namespace AsaasBlazorAuthentication.Application.Users.CreateUser;

public sealed record CreateUserCommand(
    string Name,
    string Email,
    string PhoneNumber,
    string Password,
    string PasswordCheck,
    string Role) : IRequest<Result<Guid>>;