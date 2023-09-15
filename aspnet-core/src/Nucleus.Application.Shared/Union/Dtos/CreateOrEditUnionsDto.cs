
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.Union.Dtos
{
    public class CreateOrEditUnionsDto : EntityDto<int?>
    {

		[StringLength(UnionsConsts.MaxNumberLength, MinimumLength = UnionsConsts.MinNumberLength)]
		public string Number { get; set; }
		
		
		[StringLength(UnionsConsts.MaxLocalNumberLength, MinimumLength = UnionsConsts.MinLocalNumberLength)]
		public string LocalNumber { get; set; }
		
		

    }
}