using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using AsaasBlazorAuthentication.Common.Persistence.Configurations;

using AsaasBlazorAuthentication.Domain.Enrollments;

namespace AsaasBlazorAuthentication.Infrastructure.Persistence.Configurations;

internal class EnrollmentConfiguration : BaseEntityConfiguration<Enrollment>
{
    public override void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        base.Configure(builder);

        builder.Property(b => b.SubscriberId)
            .IsRequired();

        builder.Property(b => b.SubscriptionId)
            .IsRequired();

        builder.Property(b => b.Status)
            .IsRequired();

        builder.Property(b => b.StartDate)
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(b => b.ExpirationDate)
            .HasColumnType("datetime")
            .IsRequired();
    }
}