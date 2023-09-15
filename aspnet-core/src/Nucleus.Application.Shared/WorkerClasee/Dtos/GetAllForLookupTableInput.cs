using Abp.Application.Services.Dto;

namespace Nucleus.WorkerClasee.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}