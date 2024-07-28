using MediatR;

using AsaasBlazorAuthentication.Common.Results;

namespace AsaasBlazorAuthentication.Application.Users.LoginUser;

public sealed record LoginUserCommand(string Email, string Password) : IRequest<Result<LoginUserViewModel?>>;