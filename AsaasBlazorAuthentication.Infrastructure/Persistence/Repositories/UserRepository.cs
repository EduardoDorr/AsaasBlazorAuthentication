using Microsoft.EntityFrameworkCore;

using AsaasBlazorAuthentication.Domain.Users;
using AsaasBlazorAuthentication.Common.Models.Pagination;
using AsaasBlazorAuthentication.Infrastructure.Persistence.Contexts;

namespace AsaasBlazorAuthentication.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AsaasBlazorAuthenticationDbContext _dbContext;

    public UserRepository(AsaasBlazorAuthenticationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PaginationResult<User>> GetAllAsync(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var users = _dbContext.Users.AsQueryable();

        return await users.GetPaged(page, pageSize, cancellationToken);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetUserByEmailAndPasswordAsync(string email, string passwordHash)
    {
        return await _dbContext.Users.SingleOrDefaultAsync(u => u.Email.Address == email && u.Password.Content == passwordHash);
    }

    public async Task<bool> IsUniqueAsync(string email, CancellationToken cancellationToken = default)
    {
        var hasUser = await _dbContext.Users.AnyAsync(u => u.Email.Address == email);

        return !hasUser;
    }

    public void Create(User user)
    {
        _dbContext.Users.Add(user);
    }

    public void Update(User user)
    {
        _dbContext.Users.Update(user);
    }
}