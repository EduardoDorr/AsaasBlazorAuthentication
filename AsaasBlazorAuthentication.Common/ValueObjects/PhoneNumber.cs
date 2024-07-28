using System.Text.RegularExpressions;

using AsaasBlazorAuthentication.Common.Results;
using AsaasBlazorAuthentication.Common.Results.Errors;

namespace AsaasBlazorAuthentication.Common.ValueObjects;

public sealed partial record PhoneNumber
{
    public string Number { get; } = string.Empty;

    private const string _pattern = @"^[0-9]{10,11}$";

    private PhoneNumber() { }

    private PhoneNumber(string number)
    {
        Number = number;
    }

    public static Result<PhoneNumber> Create(string number)
    {
        number = FormatInput(number);

        if (string.IsNullOrWhiteSpace(number))
            return Result.Fail<PhoneNumber>(PhoneNumberErrors.PhoneNumberRequired);

        if (!IsPhoneNumber(number))
            return Result.Fail<PhoneNumber>(PhoneNumberErrors.PhoneNumberIsInvalidFormat);

        var telephone = new PhoneNumber(number);

        return Result.Ok(telephone);
    }

    public override string ToString()
    {
        return Number;
    }

    private static string FormatInput(string number)
    {
        return number.Trim()
                     .Replace("(", "")
                     .Replace(")", "")
                     .Replace("-", "")
                     .Replace(" ", "");
    }

    private static bool IsPhoneNumber(string number) =>
        Regex.IsMatch(number, _pattern);
}

public sealed record PhoneNumberErrors(string Code, string Message, ErrorType Type) : IError
{
    public static readonly Error PhoneNumberRequired =
        new("PhoneNumber.PhoneNumberRequired", "Phone number is required", ErrorType.Validation);

    public static readonly Error PhoneNumberIsInvalidFormat =
        new("PhoneNumber.PhoneNumberIsInvalidFormat", "Phone number format is invalid", ErrorType.Validation);
}