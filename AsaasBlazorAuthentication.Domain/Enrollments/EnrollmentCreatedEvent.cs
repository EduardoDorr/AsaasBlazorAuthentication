using AsaasBlazorAuthentication.Common.DomainEvents;

namespace AsaasBlazorAuthentication.Domain.Enrollments;

public sealed record EnrollmentCreatedEvent(
    Guid EnrollmentId,
    DateTime DueDate,
    decimal Value) : IDomainEvent;