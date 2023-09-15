using Abp.Application.Services.Dto;
using System;

namespace Nucleus.JCJOBS.Dtos
{
    public class GetAllJCJOBsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string STATEFilter { get; set; }

		public string LOCALITYFilter { get; set; }

		public string CLASSFilter { get; set; }

		public int? MaxCLOSEDFilter { get; set; }
		public int? MinCLOSEDFilter { get; set; }


		 public string JACCATJOBNUMFilter { get; set; }

		public GetAllJCJOBsInput()
		{
			MaxResultCount = AppConsts.DefaultPageSize;
		}
	}
}