using Abp.Application.Services.Dto;

namespace Nucleus.EquipTimetable.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}