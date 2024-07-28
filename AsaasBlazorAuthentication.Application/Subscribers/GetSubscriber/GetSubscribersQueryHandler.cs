using MediatR;
using AutoMapper;

using AsaasBlazorAuthentication.Common.Results;
using AsaasBlazorAuthentication.Common.Models.Pagination;
using AsaasBlazorAuthentication.Domain.Subscribers;
using AsaasBlazorAuthentication.Application.Subscribers.Models;

namespace AsaasBlazorAuthentication.Application.Subscribers.GetSubscriber;

public sealed class GetSubscribersQueryHandler : IRequestHandler<GetSubscribersQuery, Result<PaginationResult<SubscriberViewModel>>>
{
    private readonly ISubscriberRepository _subscriberRepository;
    private readonly IMapper _mapper;

    public GetSubscribersQueryHandler(ISubscriberRepository subscriberRepository, IMapper mapper)
    {
        _subscriberRepository = subscriberRepository;
        _mapper = mapper;
    }

    public async Task<Result<PaginationResult<SubscriberViewModel>>> Handle(GetSubscribersQuery request, CancellationToken cancellationToken)
    {
        var paginationSubscribers = await _subscriberRepository.GetAllAsync(request.Page, request.PageSize, cancellationToken);

        var subscribersViewModel = _mapper.Map<List<SubscriberViewModel>>(paginationSubscribers.Data);

        var paginationSubscribersViewModel = paginationSubscribers.Map(subscribersViewModel);

        return Result.Ok(paginationSubscribersViewModel);
    }
}