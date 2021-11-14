using Microsoft.AspNetCore.SignalR;

namespace backend.Hubs
{
    public class HubInvoker
    {
        public HubInvoker(OfferEvents offerEvents, IHubContext<ReservationsHub> reservationsHub)
        {
            offerEvents.AvailableQuantityChanged += delegate(object _, AvailableQuantityChangedEventArgs args)
            {
                reservationsHub.Clients.Group(args.Id.ToString()).SendAsync("AvailableQuantityChanged", args.AvailableQuantity);
            };
        }
    }
}