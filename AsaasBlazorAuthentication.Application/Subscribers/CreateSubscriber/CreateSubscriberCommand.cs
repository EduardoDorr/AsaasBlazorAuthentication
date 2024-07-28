using MediatR;

using AsaasBlazorAuthentication.Common.Results;

namespace AsaasBlazorAuthentication.Application.Subscribers.CreateSubscriber;

public sealed record CreateSubscriberCommand(
    string Name,
    DateTime BirthDate,
    string Cpf,
    string Email,
    string PhoneNumber,
    string Password,
    string PasswordCheck) : IRequest<Result<Guid>>;