using Abp.Application.Services.Dto;
using System;

namespace Nucleus.JobPhaseCode.Dtos
{
    public class GetAllJobPhaseCodesForExcelInput
    {
		public string Filter { get; set; }

		public string CodeFilter { get; set; }

		public string NameFilter { get; set; }


		 public string JobsNameFilter { get; set; }

		 
    }
}