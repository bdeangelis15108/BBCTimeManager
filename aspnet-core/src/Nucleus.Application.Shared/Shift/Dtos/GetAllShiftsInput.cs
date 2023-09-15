using Abp.Application.Services.Dto;
using System;

namespace Nucleus.Shift.Dtos
{
    public class GetAllShiftsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public DateTime? MaxScheduledStartFilter { get; set; }
		public DateTime? MinScheduledStartFilter { get; set; }

		public DateTime? MaxScheduledEndFilter { get; set; }
		public DateTime? MinScheduledEndFilter { get; set; }

		public string NameFilter { get; set; }


		 public string JobsNameFilter { get; set; }

		public GetAllShiftsInput()
		{
			MaxResultCount = AppConsts.DefaultPageSize;
		}
	}
}