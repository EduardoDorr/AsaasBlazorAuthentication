using MediatR;

using AsaasBlazorAuthentication.Common.Results;
using AsaasBlazorAuthentication.Application.Subscribers.Models;

namespace AsaasBlazorAuthentication.Application.Subscribers.GetSubscriberById;

public sealed record GetSubscriberByIdQuery(Guid Id) : IRequest<Result<SubscriberDetailsViewModel?>>;