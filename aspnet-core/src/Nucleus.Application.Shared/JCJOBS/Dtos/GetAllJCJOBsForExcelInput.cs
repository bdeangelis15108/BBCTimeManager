using Abp.Application.Services.Dto;
using System;

namespace Nucleus.JCJOBS.Dtos
{
    public class GetAllJCJOBsForExcelInput
    {
		public string Filter { get; set; }

		public string STATEFilter { get; set; }

		public string LOCALITYFilter { get; set; }

		public string CLASSFilter { get; set; }

		public int? MaxCLOSEDFilter { get; set; }
		public int? MinCLOSEDFilter { get; set; }


		 public string JACCATJOBNUMFilter { get; set; }

		 
    }
}