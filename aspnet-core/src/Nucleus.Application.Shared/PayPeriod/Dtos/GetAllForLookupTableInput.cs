using Abp.Application.Services.Dto;

namespace Nucleus.PayPeriod.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
        public GetAllForLookupTableInput()
        {
            MaxResultCount = AppConsts.DefaultPageSize;
        }
    }
}