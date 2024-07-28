using MediatR;

using AsaasBlazorAuthentication.Common.Results;

namespace AsaasBlazorAuthentication.Application.Subscribers.DeleteSubscriber;

public sealed record DeleteSubscriberCommand(Guid Id) : IRequest<Result>;