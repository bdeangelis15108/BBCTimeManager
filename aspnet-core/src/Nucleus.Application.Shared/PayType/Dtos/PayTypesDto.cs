
using System;
using Abp.Application.Services.Dto;

namespace Nucleus.PayType.Dtos
{
    public class PayTypesDto : EntityDto
    {
		public string DescriptionCode => Code + " (" + Description +")";
		public string Code { get; set; }

		public string Description { get; set; }

		public decimal Multiplier { get; set; }

		public bool Section1 { get; set; }

		public bool Section2 { get; set; }

		public bool Section3 { get; set; }



    }
}