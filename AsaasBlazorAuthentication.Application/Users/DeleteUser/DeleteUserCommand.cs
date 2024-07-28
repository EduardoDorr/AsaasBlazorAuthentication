using MediatR;

using AsaasBlazorAuthentication.Common.Results;

namespace AsaasBlazorAuthentication.Application.Users.DeleteUser;

public sealed record DeleteUserCommand(Guid Id) : IRequest<Result>;