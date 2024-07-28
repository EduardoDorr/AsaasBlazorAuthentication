using MediatR;
using AutoMapper;

using AsaasBlazorAuthentication.Common.Results;
using AsaasBlazorAuthentication.Domain.Subscribers;
using AsaasBlazorAuthentication.Application.Subscribers.Models;

namespace AsaasBlazorAuthentication.Application.Subscribers.GetSubscriberById;

public sealed class GetSubscriberByIdQueryHandler : IRequestHandler<GetSubscriberByIdQuery, Result<SubscriberDetailsViewModel?>>
{
    private readonly ISubscriberRepository _subscriberRepository;
    private readonly IMapper _mapper;

    public GetSubscriberByIdQueryHandler(ISubscriberRepository subscriberRepository, IMapper mapper)
    {
        _subscriberRepository = subscriberRepository;
        _mapper = mapper;
    }

    public async Task<Result<SubscriberDetailsViewModel?>> Handle(GetSubscriberByIdQuery request, CancellationToken cancellationToken)
    {
        var subscriber = await _subscriberRepository.GetByIdAsync(request.Id, cancellationToken);

        if (subscriber is null)
            return Result.Fail<SubscriberDetailsViewModel?>(SubscriberErrors.NotFound);

        var subscriberViewModel = _mapper.Map<SubscriberDetailsViewModel?>(subscriber);

        return Result.Ok(subscriberViewModel);
    }
}