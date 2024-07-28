using MediatR;

namespace AsaasBlazorAuthentication.Common.DomainEvents;

public interface IDomainEventHandler<TDomainEvent>
    : INotificationHandler<TDomainEvent> where TDomainEvent : IDomainEvent
{
}