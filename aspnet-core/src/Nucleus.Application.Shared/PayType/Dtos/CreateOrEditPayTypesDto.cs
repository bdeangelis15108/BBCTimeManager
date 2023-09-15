
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.PayType.Dtos
{
    public class CreateOrEditPayTypesDto : EntityDto<int?>
    {

		[StringLength(PayTypesConsts.MaxCodeLength, MinimumLength = PayTypesConsts.MinCodeLength)]
		public string Code { get; set; }
		
		
		[StringLength(PayTypesConsts.MaxDescriptionLength, MinimumLength = PayTypesConsts.MinDescriptionLength)]
		public string Description { get; set; }
		
		
		public decimal Multiplier { get; set; }
		
		
		public bool Section1 { get; set; }
		
		
		public bool Section2 { get; set; }
		
		
		public bool Section3 { get; set; }
		
		

    }
}