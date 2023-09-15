using Abp.Application.Services.Dto;
using System;

namespace Nucleus.Job.Dtos
{
    public class GetAllJobsesForExcelInput
    {
		public string Filter { get; set; }

		public string CodeFilter { get; set; }

		public string NameFilter { get; set; }

		public DateTime? MaxStartDateFilter { get; set; }
		public DateTime? MinStartDateFilter { get; set; }

		public DateTime? MaxEndDateFilter { get; set; }
		public DateTime? MinEndDateFilter { get; set; }

		public int? MaxStatusFilter { get; set; }
		public int? MinStatusFilter { get; set; }


		 public string AddressesLinne1Filter { get; set; }

		 		 public string JobClassesNameFilter { get; set; }

		 
    }
}