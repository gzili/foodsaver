using System;

namespace backend.Hubs
{
    public class OfferEvents
    {
        public event EventHandler<AvailableQuantityChangedEventArgs> AvailableQuantityChanged;

        public void OnAvailableQuantityChanged(AvailableQuantityChangedEventArgs args) =>
            AvailableQuantityChanged?.Invoke(null, args);
    }
}