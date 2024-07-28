using AsaasBlazorAuthentication.Common.DomainEvents;
using AsaasBlazorAuthentication.Common.Persistence.UnitOfWork;
using AsaasBlazorAuthentication.Domain.Enrollments;
using AsaasBlazorAuthentication.Domain.EnrollmentPayments;
using AsaasBlazorAuthentication.Domain.Subscribers;

namespace AsaasBlazorAuthentication.Application.EnrollmentPayments.EnrollmentPaymentStatusUpdated;

public sealed class EnrollmentPaymentStatusUpdatedEventHandler : IDomainEventHandler<EnrollmentPaymentStatusUpdatedEvent>
{
    private readonly IEnrollmentPaymentRepository _enrollmentPaymentRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly ISubscriberRepository _subscriberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EnrollmentPaymentStatusUpdatedEventHandler(
        IEnrollmentPaymentRepository enrollmentPaymentRepository,
        IEnrollmentRepository enrollmentRepository,
        ISubscriberRepository subscriberRepository,
        IUnitOfWork unitOfWork)
    {
        _enrollmentPaymentRepository = enrollmentPaymentRepository;
        _enrollmentRepository = enrollmentRepository;
        _subscriberRepository = subscriberRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(EnrollmentPaymentStatusUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var enrollmentPayment = await _enrollmentPaymentRepository.GetByExternalId(notification.PaymentId, cancellationToken);

        if (enrollmentPayment is null)
        {
            // do something
            return;
        }

        var enrollment = await _enrollmentRepository.GetByIdAsync(enrollmentPayment.EnrollmentId, cancellationToken);

        if (enrollment is null)
        {
            // do something
            return;
        }

        var subscriber = await _subscriberRepository.GetByExternalId(notification.CustomerId, cancellationToken);

        if (subscriber is null)
        {
            // do something
            return;
        }

        if (subscriber.Id != enrollment.SubscriberId)
        {
            // do something
            return;
        }

        enrollmentPayment.SetStatus(notification.PaymentStatus);

        if (notification.PaymentStatus == EnrollmentPaymentStatus.Success)
            enrollment.SetActivedStatus();

        _enrollmentRepository.Update(enrollment);
        _enrollmentPaymentRepository.Update(enrollmentPayment);

        var updated = await _unitOfWork.SaveChangesAsync() > 0;

        if (!updated)
        {
            // do something
            return;
        }
    }
}