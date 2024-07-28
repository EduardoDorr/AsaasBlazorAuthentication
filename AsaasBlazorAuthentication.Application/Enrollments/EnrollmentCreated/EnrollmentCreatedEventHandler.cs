using AsaasBlazorAuthentication.Common.DomainEvents;
using AsaasBlazorAuthentication.Common.Persistence.UnitOfWork;
using AsaasBlazorAuthentication.Domain.Enrollments;
using AsaasBlazorAuthentication.Domain.EnrollmentPayments;

using AsaasBlazorAuthentication.Application.Abstractions.Models;
using AsaasBlazorAuthentication.Application.Abstractions.PaymentGateway;
using AsaasBlazorAuthentication.Domain.Subscribers;

namespace AsaasBlazorAuthentication.Application.Enrollments.EnrollmentCreated;

public sealed class EnrollmentCreatedEventHandler : IDomainEventHandler<EnrollmentCreatedEvent>
{
    private readonly IEnrollmentPaymentRepository _enrollmentPaymentRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly ISubscriberRepository _subscriberRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentGateway _paymentGateway;

    public EnrollmentCreatedEventHandler(
        IEnrollmentPaymentRepository enrollmentPaymentRepository,
        IEnrollmentRepository enrollmentRepository,
        ISubscriberRepository subscriberRepository,
        IUnitOfWork unitOfWork,
        IPaymentGateway paymentGateway)
    {
        _enrollmentPaymentRepository = enrollmentPaymentRepository;
        _enrollmentRepository = enrollmentRepository;
        _subscriberRepository = subscriberRepository;
        _unitOfWork = unitOfWork;
        _paymentGateway = paymentGateway;
    }

    public async Task Handle(EnrollmentCreatedEvent notification, CancellationToken cancellationToken)
    {
        var enrollment = await _enrollmentRepository.GetByIdAsync(notification.EnrollmentId, cancellationToken);

        if (enrollment is null)
        {
            // do something
            return;
        }

        var subscriber = await _subscriberRepository.GetByIdAsync(enrollment.SubscriberId, cancellationToken);

        if (subscriber is null)
        {
            // do something
            return;
        }

        var createPaymentModel =
            new CreatePaymentModel(
                subscriber.PaymentGatewayClientId,
                notification.Value,
                notification.DueDate,
                enrollment.Subscription.Name);

        var paymentCreatedResult =
            await _paymentGateway.CreatePaymentAsync(createPaymentModel);

        if (!paymentCreatedResult.Success)
        {
            // do somenthing
            return;
        }

        var paymentCreated = paymentCreatedResult.Value;

        var enrollmentPaymentResult =
            EnrollmentPayment.Create(
                notification.EnrollmentId,
                enrollment.Subscription.Name,
                notification.Value,
                notification.DueDate,
                paymentCreated.InvoiceUrl,
                paymentCreated.PaymentId);

        if (!enrollmentPaymentResult.Success)
        {
            // do somenthing
            return;
        }

        _enrollmentPaymentRepository.Create(enrollmentPaymentResult.Value!);

        var created = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

        if (!created)
        {
            // do somenthing
            return;
        }
    }
}