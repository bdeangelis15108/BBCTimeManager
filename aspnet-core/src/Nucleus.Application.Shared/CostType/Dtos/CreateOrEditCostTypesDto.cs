
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.CostType.Dtos
{
    public class CreateOrEditCostTypesDto : EntityDto<int?>
    {

		[StringLength(CostTypesConsts.MaxNameLength, MinimumLength = CostTypesConsts.MinNameLength)]
		public string Name { get; set; }
		
		
		[StringLength(CostTypesConsts.MaxCodeLength, MinimumLength = CostTypesConsts.MinCodeLength)]
		public string Code { get; set; }
		
		

    }
}