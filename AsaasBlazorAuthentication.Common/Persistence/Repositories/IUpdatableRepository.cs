using AsaasBlazorAuthentication.Common.Entities;

namespace AsaasBlazorAuthentication.Common.Persistence.Repositories;

public interface IUpdatableRepository<TEntity> where TEntity : BaseEntity
{
    void Update(TEntity entity);
}