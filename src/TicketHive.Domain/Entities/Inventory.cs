using System;

namespace TicketHive.Domain.Entities
{
    public class Inventory
    {
        private readonly object _stockLock = new();

        public Guid Id { get; private set; }
        public Guid TicketId { get; private set; }
        public int RemainingQuantity { get; private set; }
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

        private Inventory() { } 

        public Inventory(Guid ticketId, int totalQuantity)
        {
            Id = Guid.NewGuid();
            TicketId = ticketId;
            RemainingQuantity = totalQuantity;
        }

        public void UpdateTotalQuantity(int newTotal)
        {
            lock (_stockLock)
            {
                if (RemainingQuantity > newTotal)
                    throw new InvalidOperationException("New total cannot be less than tickets already sold");

                RemainingQuantity = newTotal;
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public void AdjustStock(int quantity)
        {
            lock (_stockLock)
            {
                if (RemainingQuantity + quantity < 0)
                    throw new InvalidOperationException("Insufficient tickets available");
                RemainingQuantity += quantity;
                UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
