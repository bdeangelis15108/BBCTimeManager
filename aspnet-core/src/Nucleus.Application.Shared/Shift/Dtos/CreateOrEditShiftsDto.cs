
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.Shift.Dtos
{
    public class CreateOrEditShiftsDto : EntityDto<int?>
    {

		public DateTime? ScheduledStart { get; set; }
		
		
		public DateTime? ScheduledEnd { get; set; }
		
		
		public string Name { get; set; }
		
		
		 public int JobsId { get; set; }
		 
		 
    }
}