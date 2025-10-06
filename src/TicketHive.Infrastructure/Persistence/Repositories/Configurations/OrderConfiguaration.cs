using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketHive.Domain.Entities;

namespace TicketHive.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.UserId)
                .IsRequired();

        builder.Property(o => o.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)"); 
        builder.Property(o => o.Status)
                .IsRequired()
                .HasConversion<string>(); 

        builder.Property(o => o.PaymentProvider)
                .HasMaxLength(50); 

        builder.Property(o => o.TransactionId)
                .HasMaxLength(255);

        builder.Property(o => o.PaymentDate);
        

        // Quan hệ 1 (Order) - nhiều (OrderItem)
        builder.HasMany(o => o.Items)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade); // Nếu Order bị xóa, OrderItem cũng bị xóa
        
        // Quan hệ với User (cho người mua)
        builder.HasOne(o => o.User)
                .WithMany() 
                .HasForeignKey(o => o.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict); 
    }
}