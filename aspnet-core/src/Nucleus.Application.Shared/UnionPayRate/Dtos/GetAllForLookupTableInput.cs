using Abp.Application.Services.Dto;

namespace Nucleus.UnionPayRate.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}