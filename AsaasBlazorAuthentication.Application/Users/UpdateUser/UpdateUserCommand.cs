using MediatR;

using AsaasBlazorAuthentication.Common.Results;

namespace AsaasBlazorAuthentication.Application.Users.UpdateUser;

public sealed record UpdateUserCommand(
    Guid Id,
    string Name,
    string Telephone) : IRequest<Result>;