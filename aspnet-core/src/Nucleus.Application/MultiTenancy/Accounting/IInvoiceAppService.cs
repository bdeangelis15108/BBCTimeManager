using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Nucleus.MultiTenancy.Accounting.Dto;

namespace Nucleus.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
