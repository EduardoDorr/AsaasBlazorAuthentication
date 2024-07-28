namespace AsaasBlazorAuthentication.Application.Subscribers.Models;

public sealed record SubscriberDetailsViewModel(
    Guid Id,
    string Name,
    DateTime BirthDate,
    string Cpf,
    string Email,
    string PhoneNumber);