using System;
using TicketHive.Domain.Common.Events;
using TicketHive.Domain.Events;

namespace TicketHive.Domain.Entities
{
    public class Ticket
    {
        public Guid Id { get; private set; }
        public Guid EventId { get; private set; }
        public string Type { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public decimal Price { get; private set; }
        public decimal? OriginalPrice { get; private set; }
        public int TotalQuantity { get; private set; }
        public int MinPurchase { get; private set; } = 1;
        public int MaxPurchase { get; private set; } = 10;
        public DateTime? SaleStartTime { get; private set; }
        public DateTime? SaleEndTime { get; private set; }
        public bool IsActive { get; private set; } = true;
        public int SortOrder { get; private set; } = 0;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

        public Guid CreatedBy { get; private set; }
        public Guid? UpdatedBy { get; private set; }

        public Event Event { get; private set; } = null!;
        public Inventory Inventory { get; private set; } = null!;

        private readonly List<IDomainEvent> _domainEvents = new();
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        private Ticket() { }

        public Ticket(
            Guid eventId,
            string type,
            string name,
            decimal price,
            int totalQuantity,
            Guid createdBy,
            int minPurchase = 1,
            int maxPurchase = 10,
            string? description = null,
            decimal? originalPrice = null,
            DateTime? saleStartTime = null,
            DateTime? saleEndTime = null,
            bool isActive = true,
            int sortOrder = 0)
        {
            ValidateTicketData(price, originalPrice, totalQuantity, minPurchase, maxPurchase, saleStartTime, saleEndTime);

            Id = Guid.NewGuid();
            EventId = eventId;
            Type = type;
            Name = name;
            Price = price;
            CreatedBy = createdBy;
            OriginalPrice = originalPrice;
            TotalQuantity = totalQuantity;
            MinPurchase = minPurchase;
            MaxPurchase = maxPurchase;
            Description = description;
            SaleStartTime = saleStartTime;
            SaleEndTime = saleEndTime;
            IsActive = isActive;
            SortOrder = sortOrder;

            Inventory = new Inventory(Id, totalQuantity);

            AddDomainEvent(new TicketCreatedDomainEvent(Id, eventId, name, price, totalQuantity));
            Touch();
        }

        // === Domain behaviors ===

        public void UpdatePrice(decimal newPrice, Guid updatedBy)
        {
            if (newPrice < 0) throw new ArgumentException("Price cannot be negative");

            Price = newPrice;
            UpdatedBy = updatedBy;
            Touch();
        }

        public void ChangeQuantity(int newTotal, Guid updatedBy)
        {
            if (newTotal < 0) throw new ArgumentException("Total quantity cannot be negative");

            TotalQuantity = newTotal;
            Inventory.UpdateTotalQuantity(newTotal);
            UpdatedBy = updatedBy;
            Touch();
        }

        public void UpdateSalePeriod(DateTime? start, DateTime? end, Guid updatedBy)
        {
            if (start.HasValue && end.HasValue && end < start)
                throw new ArgumentException("Sale end time cannot be earlier than start time");

            SaleStartTime = start;
            SaleEndTime = end;
            UpdatedBy = updatedBy;
            Touch();
        }

        public void UpdateGeneralInfo(string name, string? description, int sortOrder, Guid updatedBy)
        {
            Name = name;
            Description = description;
            SortOrder = sortOrder;
            UpdatedBy = updatedBy;
            Touch();
        }

        public void Deactivate(Guid updatedBy)
        {
            IsActive = false;
            UpdatedBy = updatedBy;
            Touch();
        }

        public bool IsOnSale(DateTime currentTime)
        {
            return IsActive
                && (!SaleStartTime.HasValue || currentTime >= SaleStartTime)
                && (!SaleEndTime.HasValue || currentTime <= SaleEndTime)
                && Inventory.RemainingQuantity > 0;
        }

        // === Private methods ===

        private void Touch() => UpdatedAt = DateTime.UtcNow;

        private static void ValidateTicketData(
            decimal price,
            decimal? originalPrice,
            int totalQuantity,
            int minPurchase,
            int maxPurchase,
            DateTime? saleStartTime,
            DateTime? saleEndTime)
        {
            if (price < 0) throw new ArgumentException("Price cannot be negative", nameof(price));
            if (originalPrice.HasValue && originalPrice < price)
                throw new ArgumentException("Original price cannot be less than current price", nameof(originalPrice));
            if (totalQuantity < 0) throw new ArgumentException("Total quantity cannot be negative", nameof(totalQuantity));
            if (minPurchase < 1) throw new ArgumentException("Minimum purchase must be at least 1", nameof(minPurchase));
            if (maxPurchase < minPurchase) throw new ArgumentException("Maximum purchase cannot be less than minimum purchase", nameof(maxPurchase));
            if (saleStartTime.HasValue && saleEndTime.HasValue && saleEndTime < saleStartTime)
                throw new ArgumentException("Sale end time cannot be earlier than start time");
        }

        private void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}
