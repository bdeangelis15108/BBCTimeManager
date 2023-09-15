using Abp.Application.Services.Dto;
using System;

namespace Nucleus.PayType.Dtos
{
    public class GetAllPayTypesesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string CodeFilter { get; set; }

		public string DescriptionFilter { get; set; }

		public decimal? MaxMultiplierFilter { get; set; }
		public decimal? MinMultiplierFilter { get; set; }

		public int Section1Filter { get; set; }

		public int Section2Filter { get; set; }

		public int Section3Filter { get; set; }

		public GetAllPayTypesesInput()
		{
			MaxResultCount = AppConsts.DefaultPageSize;
		}

	}
}