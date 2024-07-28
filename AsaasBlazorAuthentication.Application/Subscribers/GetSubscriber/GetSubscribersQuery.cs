using MediatR;

using AsaasBlazorAuthentication.Common.Results;
using AsaasBlazorAuthentication.Common.Models.Pagination;
using AsaasBlazorAuthentication.Application.Subscribers.Models;

namespace AsaasBlazorAuthentication.Application.Subscribers.GetSubscriber;

public sealed record GetSubscribersQuery(int Page = 1, int PageSize = 10) : IRequest<Result<PaginationResult<SubscriberViewModel>>>;