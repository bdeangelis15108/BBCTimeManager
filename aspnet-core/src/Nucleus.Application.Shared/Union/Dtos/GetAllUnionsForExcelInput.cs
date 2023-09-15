using Abp.Application.Services.Dto;
using System;

namespace Nucleus.Union.Dtos
{
    public class GetAllUnionsForExcelInput
    {
		public string Filter { get; set; }

		public string NumberFilter { get; set; }

		public string LocalNumberFilter { get; set; }



    }
}