using AsaasBlazorAuthentication.Common.Results.Errors;

namespace AsaasBlazorAuthentication.Domain.Subscribers;

public sealed record SubscriberErrors(string Code, string Message, ErrorType Type) : IError
{
    public static readonly Error CannotBeCreated =
        new("Subscriber.CannotBeCreated", "Something went wrong and the Subscriber cannot be created", ErrorType.Failure);

    public static readonly Error CannotBeUpdated =
        new("Subscriber.CannotBeUpdated", "Something went wrong and the Subscriber cannot be updated", ErrorType.Failure);

    public static readonly Error CannotBeDeleted =
        new("Subscriber.CannotBeDeleted", "Something went wrong and the Subscriber cannot be deleted", ErrorType.Failure);

    public static readonly Error NotFound =
        new("Subscriber.NotFound", "Could not find an active Subscriber", ErrorType.NotFound);

    public static readonly Error IsNotUnique =
        new("Subscriber.IsNotUnique", "The Subscriber's CPF is already taken", ErrorType.Conflict);
}