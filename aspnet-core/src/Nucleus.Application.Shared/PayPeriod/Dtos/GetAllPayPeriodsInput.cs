using Abp.Application.Services.Dto;
using System;

namespace Nucleus.PayPeriod.Dtos
{
    public class GetAllPayPeriodsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public DateTime? MaxStartDateFilter { get; set; }
		public DateTime? MinStartDateFilter { get; set; }

		public DateTime? MaxEndDateFilter { get; set; }
		public DateTime? MinEndDateFilter { get; set; }

		public string NameFilter { get; set; }

		public int IsActiveFilter { get; set; }


		public GetAllPayPeriodsInput()
		{
			MaxResultCount = AppConsts.DefaultPageSize;
		}
	}
}