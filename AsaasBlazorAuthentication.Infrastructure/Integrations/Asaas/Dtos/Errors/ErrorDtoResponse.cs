using System.Text.Json.Serialization;

using AsaasBlazorAuthentication.Common.Results.Errors;

namespace AsaasBlazorAuthentication.Infrastructure.Integrations.Asaas.Dtos.Errors;

internal sealed record ErrorDtoResponse(
    [property: JsonPropertyName("errors")] IReadOnlyList<Error> Errors);

internal sealed record Error(
        [property: JsonPropertyName("code")] string Code,
        [property: JsonPropertyName("description")] string Description);

internal static class ErrorDtoResponseExtension
{
    public static List<Common.Results.Errors.Error> ToError(this ErrorDtoResponse? response)
    {
        return response is not null
             ? response.Errors.Select(r => new Common.Results.Errors.Error(r.Code, r.Description, ErrorType.Validation)).ToList()
             : [];
    }
}