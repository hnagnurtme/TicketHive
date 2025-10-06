using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketHive.Domain.Entities;

namespace TicketHive.Infrastructure.Persistence.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("tickets");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(255);

        builder.Property(t => t.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

        builder.Property(t => t.TotalQuantity)
                .IsRequired();

        builder.Property(t => t.MinPurchase)
                .IsRequired();

        builder.Property(t => t.MaxPurchase)
                .IsRequired();

        builder.Property(t => t.Description)
                .HasMaxLength(1000);

        builder.Property(t => t.OriginalPrice)
                .HasColumnType("decimal(18,2)");

        builder.Property(t => t.SaleStartTime);

        builder.Property(t => t.SaleEndTime);

        builder.Property(t => t.IsActive)
                .IsRequired();

        builder.Property(t => t.SortOrder)
                .IsRequired();

        builder.HasOne(t => t.Event)
                .WithMany()
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.Cascade);
    }
}