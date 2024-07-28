using AsaasBlazorAuthentication.Common.Entities;

namespace AsaasBlazorAuthentication.Common.Persistence.Repositories;

public interface ICreatableRepository<TEntity> where TEntity : BaseEntity
{
    void Create(TEntity entity);
}