﻿using Microsoft.EntityFrameworkCore;

using AsaasBlazorAuthentication.Common.Models.Pagination;

using AsaasBlazorAuthentication.Domain.EnrollmentPayments;
using AsaasBlazorAuthentication.Infrastructure.Persistence.Contexts;

namespace AsaasBlazorAuthentication.Infrastructure.Persistence.Repositories;

public class EnrollmentPaymentRepository : IEnrollmentPaymentRepository
{
    private readonly AsaasBlazorAuthenticationDbContext _dbContext;

    public EnrollmentPaymentRepository(AsaasBlazorAuthenticationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PaginationResult<EnrollmentPayment>> GetAllAsync(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var users = _dbContext.EnrollmentPayments.AsQueryable();

        return await users.GetPaged(page, pageSize, cancellationToken);
    }

    public async Task<EnrollmentPayment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.EnrollmentPayments
            .Include(c => c.Enrollment)
            .Include(c => c.Enrollment)
            .ThenInclude(c => c.Subscriber)
            .SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<EnrollmentPayment?> GetByExternalId(string externalId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.EnrollmentPayments
            .SingleOrDefaultAsync(e => e.PaymentId == externalId, cancellationToken);
    }

    public void Create(EnrollmentPayment enrollmentPayment)
    {
        _dbContext.EnrollmentPayments.Add(enrollmentPayment);
    }

    public void Update(EnrollmentPayment enrollmentPayment)
    {
        _dbContext.EnrollmentPayments.Update(enrollmentPayment);
    }
}