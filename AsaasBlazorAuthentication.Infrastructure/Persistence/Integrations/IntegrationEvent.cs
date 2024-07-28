using AsaasBlazorAuthentication.Common.Entities;

namespace AsaasBlazorAuthentication.Infrastructure.Persistence.Integrations;

public class IntegrationEvent : BaseEntity
{
    public string EventType { get; private set; }
    public string Payload { get; private set; }
}