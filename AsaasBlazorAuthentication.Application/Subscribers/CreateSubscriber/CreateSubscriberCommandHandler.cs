using MediatR;

using AsaasBlazorAuthentication.Common.Auth;
using AsaasBlazorAuthentication.Common.Results;
using AsaasBlazorAuthentication.Common.Persistence.UnitOfWork;
using AsaasBlazorAuthentication.Domain.Subscribers;

namespace AsaasBlazorAuthentication.Application.Subscribers.CreateSubscriber;

public sealed class CreateSubscriberCommandHandler : IRequestHandler<CreateSubscriberCommand, Result<Guid>>
{
    private readonly ISubscriberRepository _subscriberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSubscriberCommandHandler(ISubscriberRepository subscriberRepository, IUnitOfWork unitOfWork)
    {
        _subscriberRepository = subscriberRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateSubscriberCommand request, CancellationToken cancellationToken)
    {
        var isUnique = await _subscriberRepository.IsUniqueAsync(request.Cpf, request.Email, cancellationToken);

        if (!isUnique)
            return Result.Fail<Guid>(SubscriberErrors.IsNotUnique);

        var subscriberResult = Subscriber.Create(
            request.Name,
            request.BirthDate,
            request.Cpf,
            request.Email,
            request.PhoneNumber,
            request.Password);

        if (!subscriberResult.Success)
            return Result.Fail<Guid>(subscriberResult.Errors);

        _subscriberRepository.Create(subscriberResult.Value!);

        var created = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

        if (!created)
            return Result.Fail<Guid>(SubscriberErrors.CannotBeCreated);

        return Result.Ok(subscriberResult.Value!.Id);
    }
}