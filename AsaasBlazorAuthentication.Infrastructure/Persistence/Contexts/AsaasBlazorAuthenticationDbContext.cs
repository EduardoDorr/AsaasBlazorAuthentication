using System.Reflection;

using Microsoft.EntityFrameworkCore;

using AsaasBlazorAuthentication.Common.Persistence.Outbox;
using AsaasBlazorAuthentication.Common.Persistence.Configurations;

using AsaasBlazorAuthentication.Domain.Users;
using AsaasBlazorAuthentication.Domain.Constants;
using AsaasBlazorAuthentication.Domain.Enrollments;
using AsaasBlazorAuthentication.Domain.Subscribers;
using AsaasBlazorAuthentication.Domain.Subscriptions;
using AsaasBlazorAuthentication.Domain.EnrollmentPayments;

namespace AsaasBlazorAuthentication.Infrastructure.Persistence.Contexts;

public class AsaasBlazorAuthenticationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Subscriber> Subscribers { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<EnrollmentPayment> EnrollmentPayments { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    public AsaasBlazorAuthenticationDbContext(DbContextOptions<AsaasBlazorAuthenticationDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SchemaConstants.AsaasBlazorAuthenticationSchema);
        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}