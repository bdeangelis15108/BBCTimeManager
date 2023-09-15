using Abp.Application.Services.Dto;
using System;

namespace Nucleus.Resource.Dtos
{
    public class GetAllResourcesesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public string TypeFilter { get; set; }

		public decimal? MaxCostPerHourFilter { get; set; }
		public decimal? MinCostPerHourFilter { get; set; }

		public decimal? MaxCostPerUserFilter { get; set; }
		public decimal? MinCostPerUserFilter { get; set; }

		public decimal? MaxCostPerDayFilter { get; set; }
		public decimal? MinCostPerDayFilter { get; set; }

		public string ResourceNumberFilter { get; set; }

		public GetAllResourcesesInput()
		{
			MaxResultCount = AppConsts.DefaultPageSize;
		}

	}
}