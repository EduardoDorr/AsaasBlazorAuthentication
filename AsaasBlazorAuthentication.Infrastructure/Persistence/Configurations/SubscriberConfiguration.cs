using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using AsaasBlazorAuthentication.Common.Persistence.Configurations;
using AsaasBlazorAuthentication.Domain.Subscribers;

namespace AsaasBlazorAuthentication.Infrastructure.Persistence.Configurations;

internal class SubscriberConfiguration : BaseEntityConfiguration<Subscriber>
{
    public override void Configure(EntityTypeBuilder<Subscriber> builder)
    {
        base.Configure(builder);

        builder.Property(b => b.UserId)
            .IsRequired();

        builder.Property(b => b.BirthDate)
            .HasColumnType("datetime")
            .IsRequired();

        builder.OwnsOne(d => d.Cpf,
            cpf =>
            {
                cpf.Property(d => d.Number)
                   .HasColumnName("Cpf")
                   .HasMaxLength(11)
                   .IsRequired();

                cpf.HasIndex(d => d.Number)
                   .IsUnique();
            });

        builder.Property(b => b.PaymentGatewayClientId)
            .HasMaxLength(50);

        builder.HasOne(b => b.User)
            .WithOne()
            .HasForeignKey<Subscriber>(b => b.UserId);

        builder.HasMany(b => b.Enrollments)
            .WithOne(b => b.Subscriber)
            .HasForeignKey(b => b.SubscriberId);
    }
}