using Microsoft.EntityFrameworkCore;

using AsaasBlazorAuthentication.Common.Models.Pagination;

using AsaasBlazorAuthentication.Domain.Enrollments;
using AsaasBlazorAuthentication.Infrastructure.Persistence.Contexts;

namespace AsaasBlazorAuthentication.Infrastructure.Persistence.Repositories;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly AsaasBlazorAuthenticationDbContext _dbContext;

    public EnrollmentRepository(AsaasBlazorAuthenticationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PaginationResult<Enrollment>> GetAllAsync(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var users = _dbContext.Enrollments.AsQueryable();

        return await users.GetPaged(page, pageSize, cancellationToken);
    }

    public async Task<Enrollment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Enrollments
            .Include(c => c.Subscriber)
            .SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<bool> IsUniqueAsync(Guid userId, Guid subscriptionId, CancellationToken cancellationToken = default)
    {
        var hasUser = await _dbContext.Enrollments.AnyAsync(e => e.SubscriberId == userId && e.SubscriptionId == subscriptionId);

        return !hasUser;
    }

    public void Create(Enrollment enrollment)
    {
        _dbContext.Enrollments.Add(enrollment);
    }

    public void Update(Enrollment enrollment)
    {
        _dbContext.Enrollments.Update(enrollment);
    }
}