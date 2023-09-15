using Abp.Application.Services.Dto;

namespace Nucleus.Account.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}