
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.ResourceEquipmentInfo.Dtos
{
    public class CreateOrEditResourceEquipmentInfosDto : EntityDto<int?>
    {

		[StringLength(ResourceEquipmentInfosConsts.MaxNameLength, MinimumLength = ResourceEquipmentInfosConsts.MinNameLength)]
		public string Name { get; set; }
		
		

    }
}