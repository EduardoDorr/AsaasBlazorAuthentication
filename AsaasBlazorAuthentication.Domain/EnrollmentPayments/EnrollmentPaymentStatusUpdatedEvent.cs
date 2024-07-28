using AsaasBlazorAuthentication.Common.DomainEvents;

namespace AsaasBlazorAuthentication.Domain.EnrollmentPayments;

public sealed record EnrollmentPaymentStatusUpdatedEvent(
    string PaymentId,
    string CustomerId,
    EnrollmentPaymentStatus PaymentStatus) : IDomainEvent;