using Abp.Application.Services.Dto;
using System;

namespace Nucleus.EmployeeUnion.Dtos
{
    public class GetAllEmployeeUnionsForExcelInput
    {
		public string Filter { get; set; }

		public string LocalNumberFilter { get; set; }


		 public string UnionsNumberFilter { get; set; }

		 		 public string ResourcesNameFilter { get; set; }

		 
    }
}