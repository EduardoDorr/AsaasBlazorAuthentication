using AsaasBlazorAuthentication.Common.Persistence.Repositories;

namespace AsaasBlazorAuthentication.Domain.Users;

public interface IUserRepository
    : IReadableRepository<User>,
      ICreatableRepository<User>,
      IUpdatableRepository<User>
{
    Task<User?> GetUserByEmailAndPasswordAsync(string email, string passwordHash);
    Task<bool> IsUniqueAsync(string email, CancellationToken cancellationToken = default);
}