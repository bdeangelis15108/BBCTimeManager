using Abp.Application.Services.Dto;
using System;

namespace Nucleus.PRDEDRATES.Dtos
{
    public class GetAllPRDEDRATESInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string UNIONLOCALFilter { get; set; }

		public string CLASSFilter { get; set; }

		public int? MaxDEDTYPEFilter { get; set; }
		public int? MinDEDTYPEFilter { get; set; }

		public decimal? MaxPERHRFilter { get; set; }
		public decimal? MinPERHRFilter { get; set; }


		 public string PRCLASSUNIONNUMFilter { get; set; }

		public GetAllPRDEDRATESInput()
		{
			MaxResultCount = AppConsts.DefaultPageSize;
		}
	}
}