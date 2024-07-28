using AsaasBlazorAuthentication.Common.Persistence.Repositories;

namespace AsaasBlazorAuthentication.Domain.Subscribers;

public interface ISubscriberRepository
    : IReadableRepository<Subscriber>,
      ICreatableRepository<Subscriber>,
      IUpdatableRepository<Subscriber>
{
    Task<bool> IsUniqueAsync(string cpf, string email, CancellationToken cancellationToken = default);
    Task<Subscriber?> GetByExternalId(string externalId, CancellationToken cancellationToken = default);
}