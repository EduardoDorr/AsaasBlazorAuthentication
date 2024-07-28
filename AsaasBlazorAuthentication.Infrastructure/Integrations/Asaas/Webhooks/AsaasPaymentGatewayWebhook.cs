using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using MediatR;

using AsaasBlazorAuthentication.Common.Options;
using AsaasBlazorAuthentication.Domain.EnrollmentPayments;
using AsaasBlazorAuthentication.Infrastructure.Integrations.Asaas.Dtos.Payments;

namespace AsaasBlazorAuthentication.Infrastructure.Integrations.Asaas.Webhooks;

[ApiController]
[Route("api/webhooks/incoming/asaas")]
[ApiExplorerSettings(IgnoreApi = true)]
public class AsaasPaymentGatewayWebhook : ControllerBase
{
    private readonly IPublisher _publisher;
    private readonly string _accessToken;

    public AsaasPaymentGatewayWebhook(IPublisher publisher, IOptions<AsaasApiOptions> asaasApiOptions)
    {
        _publisher = publisher;
        _accessToken = asaasApiOptions.Value.WebhookToken;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromHeader(Name = "asaas-access-token")] string accessToken, [FromBody] PaymentWebhookEvent paymentWebhookEvent)
    {
        if (accessToken != _accessToken)
            return Unauthorized();

        if (!Enum.TryParse(paymentWebhookEvent.@event, out PaymentWebhookEventType paymentWebhookEventType))
            return BadRequest("Event type not accepted");

        var enrollmentPaymentStatusUpdatedEvent =
            new EnrollmentPaymentStatusUpdatedEvent(
                paymentWebhookEvent.payment.id,
                paymentWebhookEvent.payment.customer,
                paymentWebhookEventType.ToEnrollmentPaymentStatus());

        await _publisher.Publish(enrollmentPaymentStatusUpdatedEvent);

        return Ok();
    }
}