using Abp.Application.Services.Dto;

namespace Nucleus.ShiftResource.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}