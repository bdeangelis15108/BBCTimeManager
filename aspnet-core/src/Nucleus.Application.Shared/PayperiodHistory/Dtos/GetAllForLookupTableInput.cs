using Abp.Application.Services.Dto;

namespace Nucleus.PayperiodHistory.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}