using Microsoft.EntityFrameworkCore;

using AsaasBlazorAuthentication.Common.Models.Pagination;

using AsaasBlazorAuthentication.Domain.Subscriptions;

using AsaasBlazorAuthentication.Infrastructure.Persistence.Contexts;

namespace AsaasBlazorAuthentication.Infrastructure.Persistence.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly AsaasBlazorAuthenticationDbContext _dbContext;

    public SubscriptionRepository(AsaasBlazorAuthenticationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PaginationResult<Subscription>> GetAllAsync(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var users = _dbContext.Subscriptions.AsQueryable();

        return await users.GetPaged(page, pageSize, cancellationToken);
    }

    public async Task<Subscription?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Subscriptions
            .SingleOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<bool> IsUniqueAsync(string name, CancellationToken cancellationToken = default)
    {
        var hasUser = await _dbContext.Subscriptions.AnyAsync(s => s.Name == name);

        return !hasUser;
    }

    public void Create(Subscription subscription)
    {
        _dbContext.Subscriptions.Add(subscription);
    }

    public void Update(Subscription subscription)
    {
        _dbContext.Subscriptions.Update(subscription);
    }
}