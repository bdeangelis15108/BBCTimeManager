using Abp.Application.Services.Dto;

namespace Nucleus.ResourceWorkerInfo.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}