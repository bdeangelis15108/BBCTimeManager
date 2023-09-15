using Abp.Application.Services.Dto;
using System;

namespace Nucleus.Timesheet.Dtos
{
    public class GetAllTimesheetsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public DateTime? MaxCreatedDateFilter { get; set; }
		public DateTime? MinCreatedDateFilter { get; set; }

		public DateTime? MaxSubmitedDateFilter { get; set; }
		public DateTime? MinSubmitedDateFilter { get; set; }

		public string NameFilter { get; set; }


		 public string StatusesNameFilter { get; set; }

		public GetAllTimesheetsInput()
		{
			MaxResultCount = AppConsts.DefaultPageSize;
		}
	}
}