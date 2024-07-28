using AsaasBlazorAuthentication.Common.Results;

using AsaasBlazorAuthentication.Application.Abstractions.Models;

namespace AsaasBlazorAuthentication.Application.Abstractions.PaymentGateway;

public interface IPaymentGateway
{
    Task<Result<string?>> CreateClientAsync(CustomerModel model);
    Task<Result<CustomerModel?>> GetClientAsync(string id);
    Task<Result<CreatedPaymentModel?>> CreatePaymentAsync(CreatePaymentModel model);
}