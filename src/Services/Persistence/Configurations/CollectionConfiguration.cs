using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Features.Collections.Models;

namespace Services.Persistence.Configurations;

public class CollectionConfiguration
    : IEntityTypeConfiguration<Collection>
{
    public void Configure(EntityTypeBuilder<Collection> builder)
    {
        builder.ToTable("Collections");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(c => c.Slug)
            .IsRequired()
            .HasMaxLength(80);
        
        builder.Property(c => c.Description)
            .HasMaxLength(500);
        
        builder.Property(c => c.IsActive)
            .IsRequired();
    }
}