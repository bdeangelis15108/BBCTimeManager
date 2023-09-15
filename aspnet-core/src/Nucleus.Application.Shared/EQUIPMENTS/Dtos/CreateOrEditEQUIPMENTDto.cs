
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.EQUIPMENTS.Dtos
{
    public class CreateOrEditEQUIPMENTDto : EntityDto<int?>
    {

		[StringLength(EQUIPMENTConsts.MaxEQUIPNUMLength, MinimumLength = EQUIPMENTConsts.MinEQUIPNUMLength)]
		public string EQUIPNUM { get; set; }
		
		

    }
}