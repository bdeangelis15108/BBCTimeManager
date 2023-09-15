
using System;
using Abp.Application.Services.Dto;

namespace Nucleus.PayperiodHistory.Dtos
{
    public class PayperiodHistoriesDto : EntityDto
    {
		public string period { get; set; }

		public bool active { get; set; }


		 public int? PayPeriodsId { get; set; }

		 
    }
}