using AsaasBlazorAuthentication.Common.Entities;

namespace AsaasBlazorAuthentication.Common.Persistence.Repositories;

public interface IDeletableRepository<TEntity> where TEntity : BaseEntity
{
    void Delete(TEntity entity);
}