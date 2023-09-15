using Abp.Application.Services.Dto;
using System;

namespace Nucleus.CostType.Dtos
{
    public class GetAllCostTypeseForExcelInput
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public string CodeFilter { get; set; }



    }
}