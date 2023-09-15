using Abp.Application.Services.Dto;

namespace Nucleus.StatusUpdate.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}