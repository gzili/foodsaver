using backend.Models;

namespace backend.Services
{
    public interface IPushService
    {
        void NotifyAvailableQuantityChanged(int id, decimal quantity);
        void NotifyOffersChanged();
        void NotifyOfferDeleted(int id);
        void NotifyReservationsChanged(Offer offer);
    }
}