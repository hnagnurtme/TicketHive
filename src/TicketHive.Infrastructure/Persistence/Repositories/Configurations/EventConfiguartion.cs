using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketHive.Domain.Entities;

namespace TicketHive.Infrastructure.Persistence.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("events");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(e => e.Name)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(e => e.Slug)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(e => e.Location)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(e => e.Description)
               .HasMaxLength(2000);

        builder.Property(e => e.ImageUrl)
               .HasMaxLength(1000);

        builder.Property(e => e.IsFeatured)
               .HasDefaultValue(false);

        builder.Property(e => e.Status)
               .HasConversion<string>() 
               .IsRequired();

        builder.Property(e => e.CreatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(e => e.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.HasOne(e => e.Organizer)  
                .WithMany()                 
                .HasForeignKey(e => e.OrganizerId)
                .OnDelete(DeleteBehavior.Restrict);
    }
}
