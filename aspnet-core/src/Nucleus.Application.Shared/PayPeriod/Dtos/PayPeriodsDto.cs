
using System;
using Abp.Application.Services.Dto;

namespace Nucleus.PayPeriod.Dtos
{
    public class PayPeriodsDto : EntityDto
    {
		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public string Name { get; set; }

		public bool IsActive { get; set; }



    }
}