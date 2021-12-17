using backend.Models;
using backend.Services;
using Castle.DynamicProxy;

namespace backend.Interceptors
{
    public class ReservationsChangedNotifier : IInterceptor
    {
        private readonly IPushService _pushService;

        public ReservationsChangedNotifier(IPushService pushService)
        {
            _pushService = pushService;
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();

            var methodName = invocation.Method.Name;
            var args = invocation.Arguments;

            if (methodName
                is nameof(IReservationsService.Create)
                or nameof(IReservationsService.Complete)
                or nameof(IReservationsService.Delete)
                && args?[0] is Reservation reservation)
            {
                _pushService.NotifyReservationsChanged(reservation.Offer);
                if (methodName is nameof(IReservationsService.Create) or nameof(IReservationsService.Delete))
                {
                    _pushService.NotifyAvailableQuantityChanged(reservation.Offer.Id, reservation.Offer.AvailableQuantity);
                }
            }
        }
    }
}