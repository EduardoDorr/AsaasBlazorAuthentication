using AsaasBlazorAuthentication.Common.Persistence.Repositories;

namespace AsaasBlazorAuthentication.Domain.Enrollments;

public interface IEnrollmentRepository
    : IReadableRepository<Enrollment>,
      ICreatableRepository<Enrollment>,
      IUpdatableRepository<Enrollment>
{
    Task<bool> IsUniqueAsync(Guid userId, Guid subscriptionId, CancellationToken cancellationToken = default);
}