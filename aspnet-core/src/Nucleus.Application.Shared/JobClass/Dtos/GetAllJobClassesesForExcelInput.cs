using Abp.Application.Services.Dto;
using System;

namespace Nucleus.JobClass.Dtos
{
    public class GetAllJobClassesesForExcelInput
    {
		public string Filter { get; set; }

		public string CodeFilter { get; set; }

		public string NameFilter { get; set; }



    }
}