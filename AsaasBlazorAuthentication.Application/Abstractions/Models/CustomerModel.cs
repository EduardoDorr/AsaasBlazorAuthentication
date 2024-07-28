using AsaasBlazorAuthentication.Domain.Subscribers;

namespace AsaasBlazorAuthentication.Application.Abstractions.Models;

public sealed record CustomerModel(
    Guid Id,
    string Name,
    string Cpf,
    string Email,
    string Telephone);

public static class ClientModelExtension
{
    public static CustomerModel ToModel(this Subscriber subscriber) =>
        new(subscriber.Id,
            subscriber.User.Name,
            subscriber.Cpf.Number,
            subscriber.User.Email.Address,
            subscriber.User.PhoneNumber.Number);
}