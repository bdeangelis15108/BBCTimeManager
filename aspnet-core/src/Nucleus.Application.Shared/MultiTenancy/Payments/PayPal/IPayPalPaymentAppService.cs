using System.Threading.Tasks;
using Abp.Application.Services;
using Nucleus.MultiTenancy.Payments.PayPal.Dto;

namespace Nucleus.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}
