using Abp.Application.Services.Dto;
using System;

namespace Nucleus.PREMPLOYEES.Dtos
{
    public class GetAllPREMPLOYEESInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string EMPNUMFilter { get; set; }

		public string NAMEFilter { get; set; }

		public string UNIONNUMFilter { get; set; }

		public string UNIONLOCALFilter { get; set; }

		public string CLASSFilter { get; set; }

		public string WCOMPNUM1Filter { get; set; }

		public string LASTNAMEFilter { get; set; }

		public string FIRSTNAMEFilter { get; set; }

		public string STATUSFilter { get; set; }

		public decimal? MaxPAYRATEFilter { get; set; }
		public decimal? MinPAYRATEFilter { get; set; }


		public GetAllPREMPLOYEESInput()
		{
			MaxResultCount = AppConsts.DefaultPageSize;
		}
	}
}