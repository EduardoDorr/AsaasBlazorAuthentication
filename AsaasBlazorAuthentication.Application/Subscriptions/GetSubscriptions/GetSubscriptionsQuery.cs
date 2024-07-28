using MediatR;

using AsaasBlazorAuthentication.Common.Results;
using AsaasBlazorAuthentication.Common.Models.Pagination;
using AsaasBlazorAuthentication.Application.Subscriptions.Models;

namespace AsaasBlazorAuthentication.Application.Subscriptions.GetSubscriptions;

public sealed record GetSubscriptionsQuery(int Page = 1, int PageSize = 10) : IRequest<Result<PaginationResult<SubscriptionViewModel>>>;