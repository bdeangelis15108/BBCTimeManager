
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.PayPeriod.Dtos
{
    public class CreateOrEditPayPeriodsDto : EntityDto<int?>
    {

		public DateTime StartDate { get; set; }
		
		
		public DateTime EndDate { get; set; }
		
		
		public string Name { get; set; }
		
		
		public bool IsActive { get; set; }
		
		

    }
}