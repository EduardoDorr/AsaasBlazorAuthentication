namespace AsaasBlazorAuthentication.Application.Subscribers.Enroll;

public sealed record EnrollInputModel(
    Guid SubscriptionId,
    decimal Value);