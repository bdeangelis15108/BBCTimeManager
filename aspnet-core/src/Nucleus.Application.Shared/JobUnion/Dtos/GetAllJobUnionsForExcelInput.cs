using Abp.Application.Services.Dto;
using System;

namespace Nucleus.JobUnion.Dtos
{
    public class GetAllJobUnionsForExcelInput
    {
		public string Filter { get; set; }

		public string NumberFilter { get; set; }


		 public string JobsNameFilter { get; set; }

		 		 public string UnionsNumberFilter { get; set; }

		 
    }
}