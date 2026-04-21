using Intergalaxy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Intergalaxy.Infrastructure.Data.Configurations;

public class CharacterConfiguration : IEntityTypeConfiguration<Character>
{
    public void Configure(EntityTypeBuilder<Character> builder)
    {
        builder.ToTable("Characters");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ExternalId)
            .IsRequired();

        // -------------------------
        // VALUE OBJECTS (OWNED TYPES)
        // -------------------------

        builder.OwnsOne(x => x.Name, n =>
        {
            n.Property(p => p.Value)
                .HasColumnName("Name")
                .IsRequired();
        });

        builder.OwnsOne(x => x.Status, s =>
        {
            s.Property(p => p.Value)
                .HasColumnName("Status")
                .IsRequired();
        });

        builder.OwnsOne(x => x.Species, s =>
        {
            s.Property(p => p.Value)
                .HasColumnName("Species")
                .IsRequired();
        });

        builder.OwnsOne(x => x.Gender, g =>
        {
            g.Property(p => p.Value)
                .HasColumnName("Gender")
                .IsRequired();
        });

        builder.OwnsOne(x => x.Origin, o =>
        {
            o.Property(p => p.Value)
                .HasColumnName("Origin")
                .IsRequired();
        });

        // -------------------------
        // SIMPLE PROPERTIES
        // -------------------------

        builder.Property(x => x.Image)
            .IsRequired();

        builder.Property(x => x.ImportDate)
            .IsRequired();

        builder.Property(x => x.ExternalId)
            .IsRequired();

        // -------------------------
        // OPTIONAL: INDEX
        // -------------------------

        builder.HasIndex(x => x.ExternalId)
            .IsUnique();
    }
}
