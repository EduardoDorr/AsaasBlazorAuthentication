using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using AsaasBlazorAuthentication.Common.Persistence.Configurations;

using AsaasBlazorAuthentication.Domain.Users;
using AsaasBlazorAuthentication.Common.Auth;

namespace AsaasBlazorAuthentication.Infrastructure.Persistence.Configurations;

internal class UserConfiguration : BaseEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.Property(b => b.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.OwnsOne(d => d.Email,
            email =>
            {
                email.Property(d => d.Address)
                     .HasColumnName("Email")
                     .HasMaxLength(100)
                     .IsRequired();

                email.HasIndex(d => d.Address)
                     .IsUnique();
            });

        builder.OwnsOne(d => d.PhoneNumber,
            telephone =>
            {
                telephone.Property(d => d.Number)
                     .HasColumnName("PhoneNumber")
                     .HasMaxLength(11)
                     .IsRequired();
            });

        builder.OwnsOne(d => d.Password,
            password =>
            {
                password.Property(d => d.Content)
                     .HasColumnName("Password")
                     .HasMaxLength(100)
                     .IsRequired();
            });

        builder.Property(b => b.Role)
            .HasMaxLength(25)
            .IsRequired();
    }
}