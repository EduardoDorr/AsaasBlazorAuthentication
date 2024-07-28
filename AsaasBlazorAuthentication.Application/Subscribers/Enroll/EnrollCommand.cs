using MediatR;

using AsaasBlazorAuthentication.Common.Results;

namespace AsaasBlazorAuthentication.Application.Subscribers.Enroll;

public sealed record EnrollCommand(
    Guid SubscriberId,
    Guid SubscriptionId,
    decimal Value) : IRequest<Result<Guid>>;