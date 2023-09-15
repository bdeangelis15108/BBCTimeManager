using Abp.Application.Services.Dto;
using System;

namespace Nucleus.JobCategory.Dtos
{
    public class GetAllJobCategoriesForExcelInput
    {
		public string Filter { get; set; }

		public string CodeFilter { get; set; }

		public string NameFilter { get; set; }



    }
}