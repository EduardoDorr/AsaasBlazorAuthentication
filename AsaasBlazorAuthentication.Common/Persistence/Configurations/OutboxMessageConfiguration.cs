﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AsaasBlazorAuthentication.Common.Persistence.Outbox;

namespace AsaasBlazorAuthentication.Common.Persistence.Configurations;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public virtual void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(p => p.Type)
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(p => p.Error)
               .HasMaxLength(500);

        builder.Property(b => b.CreatedAt)
               .HasColumnType("datetime")
               .IsRequired();

        builder.Property(b => b.ProcessedAt)
               .HasColumnType("datetime");
    }
}