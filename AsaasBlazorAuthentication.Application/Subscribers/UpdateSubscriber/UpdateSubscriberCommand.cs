using MediatR;

using AsaasBlazorAuthentication.Common.Results;

namespace AsaasBlazorAuthentication.Application.Subscribers.UpdateSubscriber;

public sealed record UpdateSubscriberCommand(
    Guid Id,
    string Name,
    string Telephone) : IRequest<Result>;