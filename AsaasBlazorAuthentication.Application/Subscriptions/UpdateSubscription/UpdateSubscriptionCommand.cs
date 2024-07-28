using MediatR;

using AsaasBlazorAuthentication.Common.Results;

namespace AsaasBlazorAuthentication.Application.Subscriptions.UpdateSubscription;

public sealed record UpdateSubscriptionCommand(
    Guid Id,
    string Name,
    string Description,
    int Duration) : IRequest<Result>;