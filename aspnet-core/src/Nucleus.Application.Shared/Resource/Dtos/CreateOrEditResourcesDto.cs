
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.Resource.Dtos
{
    public class CreateOrEditResourcesDto : EntityDto<int?>
    {

		[Required]
		[StringLength(ResourcesConsts.MaxNameLength, MinimumLength = ResourcesConsts.MinNameLength)]
		public string Name { get; set; }
		
		
		[Required]
		[StringLength(ResourcesConsts.MaxTypeLength, MinimumLength = ResourcesConsts.MinTypeLength)]
		public string Type { get; set; }
		
		
		public decimal? CostPerHour { get; set; }
		
		
		public decimal? CostPerUser { get; set; }
		
		
		public decimal? CostPerDay { get; set; }
		
		
		public string ResourceNumber { get; set; }
		
		

    }
}