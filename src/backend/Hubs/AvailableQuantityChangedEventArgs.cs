using System;

namespace backend.Hubs
{
    public class AvailableQuantityChangedEventArgs : EventArgs
    {
        public int Id { get; }
        public decimal AvailableQuantity { get; }

        public AvailableQuantityChangedEventArgs(int id, decimal availableQuantity)
        {
            Id = id;
            AvailableQuantity = availableQuantity;
        }
    }
}