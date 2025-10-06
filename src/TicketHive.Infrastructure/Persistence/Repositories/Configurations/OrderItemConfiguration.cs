using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketHive.Domain.Entities;

namespace TicketHive.Infrastructure.Persistence.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_items");

        builder.HasKey(oi => oi.Id);

        // === Cấu hình Thuộc tính ===

        builder.Property(oi => oi.TicketId)
                .IsRequired();

        builder.Property(oi => oi.Quantity)
                .IsRequired();

        // Lưu trữ giá lịch sử
        builder.Property(oi => oi.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)"); 
        // Lưu trữ tổng tiền cho item đó
        builder.Property(oi => oi.SubTotal)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

        // === Cấu hình Quan hệ ===

        // Quan hệ với Ticket (Vé)
        builder.HasOne(oi => oi.Ticket)
                .WithMany() 
                .HasForeignKey(oi => oi.TicketId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
    }
}