using MediatR;

using AsaasBlazorAuthentication.Common.Results;
using AsaasBlazorAuthentication.Application.Subscriptions.Models;

namespace AsaasBlazorAuthentication.Application.Subscriptions.GetSubscriptionById;

public sealed record GetSubscriptionByIdQuery(Guid Id) : IRequest<Result<SubscriptionDetailsViewModel?>>;