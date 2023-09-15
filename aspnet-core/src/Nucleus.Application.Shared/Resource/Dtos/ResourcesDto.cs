
using System;
using Abp.Application.Services.Dto;

namespace Nucleus.Resource.Dtos
{
    public class ResourcesDto : EntityDto
    {
		public string Name { get; set; }

		public string Type { get; set; }

		public decimal? CostPerHour { get; set; }

		public decimal? CostPerUser { get; set; }

		public decimal? CostPerDay { get; set; }

		public string ResourceNumber { get; set; }



    }
}