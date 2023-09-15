using Abp.Application.Services.Dto;

namespace Nucleus.Status.Dtos
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