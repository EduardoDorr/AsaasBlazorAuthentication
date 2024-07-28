using AsaasBlazorAuthentication.Common.Results;
using AsaasBlazorAuthentication.Common.Entities;
using AsaasBlazorAuthentication.Common.ValueObjects;

namespace AsaasBlazorAuthentication.Domain.Users;

public class User : BaseEntity, ILogin
{
    public string Name { get; protected set; }
    public Email Email { get; protected set; }
    public PhoneNumber PhoneNumber { get; protected set; }
    public Password Password { get; protected set; }
    public string Role { get; protected set; }

    protected User() { }

    protected User(
        string name,
        Email email,
        PhoneNumber phoneNumber,
        Password password,
        string role)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        Password = password;
        Role = role;
    }

    public static Result<User> Create(
        string name,
        string email,
        string phoneNumber,
        string password,
        string role)
    {
        var emailResult = Email.Create(email);

        if (!emailResult.Success)
            return Result.Fail<User>(emailResult.Errors);

        var phoneNumberResult = PhoneNumber.Create(phoneNumber);

        if (!phoneNumberResult.Success)
            return Result.Fail<User>(phoneNumberResult.Errors);

        var passwordResult = Password.Create(password);

        if (!passwordResult.Success)
            return Result.Fail<User>(passwordResult.Errors);

        var user =
            new User(name,
                     emailResult.Value!,
                     phoneNumberResult.Value!,
                     passwordResult.Value!,
                     role);

        return Result<User>.Ok(user);
    }

    public Result Update(string name, string phoneNumber)
    {
        var phoneNumberResult = PhoneNumber.Create(phoneNumber);

        if (!phoneNumberResult.Success)
            return Result.Fail(phoneNumberResult.Errors);

        Name = name;
        PhoneNumber = phoneNumberResult.Value!;

        return Result.Ok();
    }
}