using MediatR;

using AsaasBlazorAuthentication.Common.Results;
using AsaasBlazorAuthentication.Common.Persistence.UnitOfWork;
using AsaasBlazorAuthentication.Domain.Subscribers;

namespace AsaasBlazorAuthentication.Application.Subscribers.DeleteSubscriber;

public sealed class DeleteSubscriberCommandHandler : IRequestHandler<DeleteSubscriberCommand, Result>
{
    private readonly ISubscriberRepository _subscriberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSubscriberCommandHandler(ISubscriberRepository subscriberRepository, IUnitOfWork unitOfWork)
    {
        _subscriberRepository = subscriberRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteSubscriberCommand request, CancellationToken cancellationToken)
    {
        var subscriber = await _subscriberRepository.GetByIdAsync(request.Id, cancellationToken);

        if (subscriber is null)
            return Result.Fail(SubscriberErrors.NotFound);

        subscriber.Deactivate();

        _subscriberRepository.Update(subscriber);

        var deleted = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

        if (!deleted)
            return Result.Fail(SubscriberErrors.CannotBeDeleted);

        return Result.Ok();
    }
}
