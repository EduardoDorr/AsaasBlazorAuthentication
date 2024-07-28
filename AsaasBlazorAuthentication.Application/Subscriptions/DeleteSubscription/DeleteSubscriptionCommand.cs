using MediatR;

using AsaasBlazorAuthentication.Common.Results;

namespace AsaasBlazorAuthentication.Application.Subscriptions.DeleteSubscription;

public sealed record DeleteSubscriptionCommand(Guid Id) : IRequest<Result>;