
using Intergalaxy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Intergalaxy.Infrastructure.Data.Configurations;

public class CharacterRequestConfiguration : IEntityTypeConfiguration<CharacterRequest>
{
    public void Configure(EntityTypeBuilder<CharacterRequest> builder)
    {
        builder.ToTable("CharacterRequests");

        builder.OwnsOne(x => x.Requester, r =>
        {
            r.Property(p => p.Value)
                .HasColumnName("Requester")
                .IsRequired()
                .HasMaxLength(100);
        });

        builder.OwnsOne(x => x.EventName, e =>
        {
            e.Property(p => p.Value)
                .HasColumnName("EventName")
                .IsRequired()
                .HasMaxLength(150);
        });
    }
}
