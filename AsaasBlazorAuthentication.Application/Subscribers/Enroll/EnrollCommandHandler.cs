﻿using MediatR;

using AsaasBlazorAuthentication.Common.Results;
using AsaasBlazorAuthentication.Common.Persistence.UnitOfWork;
using AsaasBlazorAuthentication.Domain.Users;
using AsaasBlazorAuthentication.Domain.Enrollments;
using AsaasBlazorAuthentication.Domain.Subscriptions;
using AsaasBlazorAuthentication.Application.Abstractions.Models;
using AsaasBlazorAuthentication.Application.Abstractions.PaymentGateway;
using AsaasBlazorAuthentication.Domain.Subscribers;

namespace AsaasBlazorAuthentication.Application.Subscribers.Enroll;

public sealed class EnrollCommandHandler : IRequestHandler<EnrollCommand, Result<Guid>>
{
    private readonly ISubscriberRepository _subscriberRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentGateway _paymentGateway;

    public EnrollCommandHandler(
        ISubscriberRepository subscriberRepository,
        ISubscriptionRepository subscriptionRepository,
        IEnrollmentRepository enrollmentRepository,
        IUnitOfWork unitOfWork,
        IPaymentGateway paymentGateway)
    {
        _subscriberRepository = subscriberRepository;
        _subscriptionRepository = subscriptionRepository;
        _enrollmentRepository = enrollmentRepository;
        _unitOfWork = unitOfWork;
        _paymentGateway = paymentGateway;
    }

    public async Task<Result<Guid>> Handle(EnrollCommand request, CancellationToken cancellationToken)
    {
        var subscriber = await _subscriberRepository.GetByIdAsync(request.SubscriberId, cancellationToken);

        if (subscriber is null)
            return Result.Fail<Guid>(UserErrors.NotFound);

        var subscription = await _subscriptionRepository.GetByIdAsync(request.SubscriptionId, cancellationToken);

        if (subscription is null)
            return Result.Fail<Guid>(SubscriptionErrors.NotFound);

        var enrollmentResult =
            Enrollment.Create(
                subscriber.Id,
                subscription.Id,
                DateTime.Today,
                DateTime.Today.AddDays(subscription.Duration),
                request.Value);

        if (!enrollmentResult.Success)
            return Result.Fail<Guid>(enrollmentResult.Errors);

        if (string.IsNullOrWhiteSpace(subscriber.PaymentGatewayClientId))
        {
            var createUserResult = await CreateUserOnPaymentGateway(subscriber);

            if (!createUserResult.Success)
                return Result.Fail<Guid>(createUserResult.Errors);
        }

        var enrollment = enrollmentResult.Value;

        _enrollmentRepository.Create(enrollment!);

        var created = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

        if (!created)
            return Result.Fail<Guid>(EnrollmentErrors.CannotBeCreated);

        return Result.Ok(enrollment!.Id);
    }

    private async Task<Result> CreateUserOnPaymentGateway(Subscriber subscriber)
    {
        var customerModel =
            new CustomerModel(
                subscriber.Id,
                subscriber.User.Name,
                subscriber.Cpf.Number,
                subscriber.User.Email.Address,
                subscriber.User.PhoneNumber.Number);

        var paymentGatewayClientIdResult = await _paymentGateway.CreateClientAsync(customerModel);

        if (!paymentGatewayClientIdResult.Success)
            return Result.Fail(paymentGatewayClientIdResult.Errors);

        subscriber.SetPaymentGatewayClientId(paymentGatewayClientIdResult.Value!);

        return Result.Ok();
    }
}