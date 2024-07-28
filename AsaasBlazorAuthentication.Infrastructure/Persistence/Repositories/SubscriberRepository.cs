using Microsoft.EntityFrameworkCore;

using AsaasBlazorAuthentication.Common.Models.Pagination;
using AsaasBlazorAuthentication.Domain.Subscribers;
using AsaasBlazorAuthentication.Infrastructure.Persistence.Contexts;

namespace AsaasBlazorAuthentication.Infrastructure.Persistence.Repositories;

public class SubscriberRepository : ISubscriberRepository
{
    private readonly AsaasBlazorAuthenticationDbContext _dbContext;

    public SubscriberRepository(AsaasBlazorAuthenticationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PaginationResult<Subscriber>> GetAllAsync(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var users = _dbContext.Subscribers.AsQueryable();

        return await users.GetPaged(page, pageSize, cancellationToken);
    }

    public async Task<Subscriber?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Subscribers
            .SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<bool> IsUniqueAsync(string cpf, string email, CancellationToken cancellationToken = default)
    {
        var hasSubscriber = await _dbContext.Subscribers
            .Include(u => u.User)
            .AnyAsync(u => u.Cpf.Number == cpf || u.User.Email.Address == email);

        return !hasSubscriber;
    }

    public async Task<Subscriber?> GetByExternalId(string externalId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Subscribers
            .SingleOrDefaultAsync(u => u.PaymentGatewayClientId == externalId, cancellationToken);
    }

    public void Create(Subscriber subscriber)
    {
        _dbContext.Subscribers.Add(subscriber);
    }

    public void Update(Subscriber subscriber)
    {
        _dbContext.Subscribers.Update(subscriber);
    }
}