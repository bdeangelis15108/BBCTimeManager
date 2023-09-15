using Abp.Application.Services.Dto;

namespace Nucleus.CostType.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}