using MediatR;

using AsaasBlazorAuthentication.Common.Results;

namespace AsaasBlazorAuthentication.Application.Subscriptions.CreateSubscription;

public sealed record CreateSubscriptionCommand(
    string Name,
    string Description,
    int Duration) : IRequest<Result<Guid>>;