using AsaasBlazorAuthentication.Common.Auth;
using AsaasBlazorAuthentication.Common.Entities;
using AsaasBlazorAuthentication.Common.Results;
using AsaasBlazorAuthentication.Common.ValueObjects;

using AsaasBlazorAuthentication.Domain.Users;
using AsaasBlazorAuthentication.Domain.Enrollments;

namespace AsaasBlazorAuthentication.Domain.Subscribers;

public sealed class Subscriber : BaseEntity
{
    public Guid UserId { get; private set; }
    public DateTime BirthDate { get; private set; }
    public Cpf Cpf { get; private set; }
    public string? PaymentGatewayClientId { get; private set; }

    public User User { get; private set; }
    public List<Enrollment> Enrollments { get; private set; } = [];

    private Subscriber() { }

    private Subscriber(
        string name,
        DateTime birthDate,
        Cpf cpf,
        User user)
    {
        UserId = user.Id;
        BirthDate = birthDate;
        Cpf = cpf;
        User = user;
    }

    public static Result<Subscriber> Create(
        string name,
        DateTime birthDate,
        string cpf,
        string email,
        string phoneNumber,
        string password)
    {
        var cpfResult = Cpf.Create(cpf);

        if (!cpfResult.Success)
            return Result.Fail<Subscriber>(cpfResult.Errors);

        var userResult =
            User.Create(
                name,
                email,
                phoneNumber,
                password,
                AuthConstants.Subscriber);

        if (!userResult.Success)
            return Result.Fail<Subscriber>(userResult.Errors);

        var subscriber =
            new Subscriber(
                name,
                birthDate,
                cpfResult.Value!,
                userResult.Value!);

        return Result<Subscriber>.Ok(subscriber);
    }

    public void SetPaymentGatewayClientId(string clientId) =>
        PaymentGatewayClientId = clientId;
}