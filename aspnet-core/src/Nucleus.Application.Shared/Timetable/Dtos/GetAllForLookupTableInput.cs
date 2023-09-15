using Abp.Application.Services.Dto;

namespace Nucleus.Timetable.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}