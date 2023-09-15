using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nucleus.Timetable.Dtos
{
    public class TimetableFilterRequestDto : PagedAndSortedResultRequestDto
    {
        public string CostCode { get; set; }
        public int PayPeriodId { get; set; }
    }
}
