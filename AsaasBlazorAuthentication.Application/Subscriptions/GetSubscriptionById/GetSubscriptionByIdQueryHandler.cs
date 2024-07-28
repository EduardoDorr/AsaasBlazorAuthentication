using MediatR;
using AutoMapper;

using AsaasBlazorAuthentication.Common.Results;
using AsaasBlazorAuthentication.Domain.Subscriptions;
using AsaasBlazorAuthentication.Application.Subscriptions.Models;

namespace AsaasBlazorAuthentication.Application.Subscriptions.GetSubscriptionById;

public sealed class GetSubscriptionByIdQueryHandler : IRequestHandler<GetSubscriptionByIdQuery, Result<SubscriptionDetailsViewModel?>>
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IMapper _mapper;

    public GetSubscriptionByIdQueryHandler(ISubscriptionRepository subscriptionRepository, IMapper mapper)
    {
        _subscriptionRepository = subscriptionRepository;
        _mapper = mapper;
    }

    public async Task<Result<SubscriptionDetailsViewModel?>> Handle(GetSubscriptionByIdQuery request, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionRepository.GetByIdAsync(request.Id, cancellationToken);

        if (subscription is null)
            return Result.Fail<SubscriptionDetailsViewModel?>(SubscriptionErrors.NotFound);

        var subscriptionViewModel = _mapper.Map<SubscriptionDetailsViewModel?>(subscription);

        return Result.Ok(subscriptionViewModel);
    }
}