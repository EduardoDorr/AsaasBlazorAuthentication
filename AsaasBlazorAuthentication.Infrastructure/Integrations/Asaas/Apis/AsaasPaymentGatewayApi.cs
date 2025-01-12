﻿using System.Text.Json;

using Microsoft.Extensions.Options;

using AsaasBlazorAuthentication.Common.Options;
using AsaasBlazorAuthentication.Common.Results;

using AsaasBlazorAuthentication.Application.Abstractions.Models;
using AsaasBlazorAuthentication.Application.Abstractions.PaymentGateway;
using AsaasBlazorAuthentication.Infrastructure.Integrations.Api;
using AsaasBlazorAuthentication.Infrastructure.Integrations.Asaas.Dtos.Errors;
using AsaasBlazorAuthentication.Infrastructure.Integrations.Asaas.Dtos.Payments;
using AsaasBlazorAuthentication.Infrastructure.Integrations.Asaas.Dtos.Customers;

namespace AsaasBlazorAuthentication.Infrastructure.Integrations.Asaas.Apis;

internal class AsaasPaymentGatewayApi : IPaymentGateway
{
    private readonly string _apiKey;
    private readonly string _baseUrl;
    private readonly string _customerEndpoint;
    private readonly string _paymentEndpoint;
    private readonly Dictionary<string, string> _headers;

    public AsaasPaymentGatewayApi(IOptions<AsaasApiOptions> asaasApiOptions)
    {
        _apiKey = asaasApiOptions.Value.ApiKey;
        _baseUrl = asaasApiOptions.Value.BaseUrl;
        _customerEndpoint = asaasApiOptions.Value.CustomerEndpoint;
        _paymentEndpoint = asaasApiOptions.Value.PaymentEndpoint;

        _headers = new Dictionary<string, string>
        {
            { "access_token", _apiKey }
        };
    }

    public async Task<Result<string?>> CreateClientAsync(CustomerModel model)
    {
        var customerDtoRequest = model.FromModel();
        var json = JsonSerializer.Serialize(customerDtoRequest);

        var response = await RestHelper
            .PostAsync<CustomerDtoResponse>(
                apiUrl: _baseUrl,
                requestUri: _customerEndpoint,
                headerParameters: _headers,
                json: json);

        if (response.IsSuccessStatusCode)
            return Result.Ok(response.Data.id);

        var error = JsonSerializer.Deserialize<ErrorDtoResponse?>(response.Content);

        return Result.Fail<string?>(error.ToError());
    }

    public async Task<Result<CustomerModel?>> GetClientAsync(string id)
    {
        var response = await RestHelper
            .GetAsync<CustomerDtoResponse>(
                apiUrl: _baseUrl,
                requestUri: $"{_customerEndpoint}/{id}",
                headerParameters: _headers);

        if (response.IsSuccessStatusCode)
            return Result.Ok(response.Data.ToModel());

        var error = JsonSerializer.Deserialize<ErrorDtoResponse?>(response.Content);

        return Result.Fail<CustomerModel?>(error.ToError());
    }

    public async Task<Result<CreatedPaymentModel?>> CreatePaymentAsync(CreatePaymentModel model)
    {
        var paymentDtoRequest = model.FromModel();
        var json = JsonSerializer.Serialize(paymentDtoRequest);

        var response = await RestHelper
            .PostAsync<PaymentDtoResponse>(
                apiUrl: _baseUrl,
                requestUri: _paymentEndpoint,
                headerParameters: _headers,
                json: json);

        if (response.IsSuccessStatusCode)
            return Result.Ok(response.Data.ToModel());

        var error = JsonSerializer.Deserialize<ErrorDtoResponse?>(response.Content);

        return Result.Fail<CreatedPaymentModel?>(error.ToError());
    }
}
