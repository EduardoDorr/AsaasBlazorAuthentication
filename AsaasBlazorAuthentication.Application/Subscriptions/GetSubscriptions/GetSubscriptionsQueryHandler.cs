using MediatR;
using AutoMapper;

using AsaasBlazorAuthentication.Common.Results;
using AsaasBlazorAuthentication.Common.Models.Pagination;
using AsaasBlazorAuthentication.Domain.Subscriptions;
using AsaasBlazorAuthentication.Application.Subscriptions.Models;

namespace AsaasBlazorAuthentication.Application.Subscriptions.GetSubscriptions;

public sealed class GetSubscriptionsQueryHandler : IRequestHandler<GetSubscriptionsQuery, Result<PaginationResult<SubscriptionViewModel>>>
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IMapper _mapper;

    public GetSubscriptionsQueryHandler(ISubscriptionRepository subscriptionRepository, IMapper mapper)
    {
        _subscriptionRepository = subscriptionRepository;
        _mapper = mapper;
    }

    public async Task<Result<PaginationResult<SubscriptionViewModel>>> Handle(GetSubscriptionsQuery request, CancellationToken cancellationToken)
    {
        var paginationSubscriptions = await _subscriptionRepository.GetAllAsync(request.Page, request.PageSize, cancellationToken);

        var subscriptionsViewModel = _mapper.Map<List<SubscriptionViewModel>>(paginationSubscriptions.Data);

        var paginationSubscriptionsViewModel = paginationSubscriptions.Map(subscriptionsViewModel);

        return Result.Ok(paginationSubscriptionsViewModel);
    }
}