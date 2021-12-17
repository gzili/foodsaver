using backend.Models;
using backend.Services;
using Castle.DynamicProxy;

namespace backend.Interceptors
{
    public class OffersChangedNotifier : IInterceptor
    {
        private readonly IPushService _pushService;

        public OffersChangedNotifier(IPushService pushService)
        {
            _pushService = pushService;
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();

            var methodName = invocation.Method.Name;
            var args = invocation.Arguments;

            if (methodName == nameof(IOffersService.Delete) && args?[0] is Offer offer)
            {
                _pushService.NotifyOfferDeleted(offer.Id);
            }
            
            if (invocation.Method.Name is nameof(IOffersService.Create) or nameof(IOffersService.Delete))
            {
                _pushService.NotifyOffersChanged();
            }
        }
    }
}