using System.Threading.Tasks;
using Abp.Application.Services;
using Nucleus.MultiTenancy.Payments.Dto;
using Nucleus.MultiTenancy.Payments.Stripe.Dto;

namespace Nucleus.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();

        Task<SubscriptionPaymentDto> GetPaymentAsync(StripeGetPaymentInput input);

        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}