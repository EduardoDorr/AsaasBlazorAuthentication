using MediatR;

using AsaasBlazorAuthentication.Common.Results;
using AsaasBlazorAuthentication.Common.Persistence.UnitOfWork;
using AsaasBlazorAuthentication.Domain.Subscribers;

namespace AsaasBlazorAuthentication.Application.Subscribers.UpdateSubscriber;

public sealed class UpdateSubscriberCommandHandler : IRequestHandler<UpdateSubscriberCommand, Result>
{
    private readonly ISubscriberRepository _subscriberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSubscriberCommandHandler(ISubscriberRepository subscriberRepository, IUnitOfWork unitOfWork)
    {
        _subscriberRepository = subscriberRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateSubscriberCommand request, CancellationToken cancellationToken)
    {
        var subscriber = await _subscriberRepository.GetByIdAsync(request.Id, cancellationToken);

        if (subscriber is null)
            return Result.Fail(SubscriberErrors.NotFound);

        //subscriber.Update(
        //    request.Name,
        //    request.Telephone);

        _subscriberRepository.Update(subscriber);

        var updated = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

        if (!updated)
            return Result.Fail(SubscriberErrors.CannotBeUpdated);

        return Result.Ok();
    }
}