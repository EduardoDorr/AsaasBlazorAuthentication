using AsaasBlazorAuthentication.Common.Persistence.Repositories;

namespace AsaasBlazorAuthentication.Domain.Subscriptions;

public interface ISubscriptionRepository
    : IReadableRepository<Subscription>,
      ICreatableRepository<Subscription>,
      IUpdatableRepository<Subscription>
{
    Task<bool> IsUniqueAsync(string name, CancellationToken cancellationToken = default);
}