
using System;
using Abp.Application.Services.Dto;

namespace Nucleus.Shift.Dtos
{
    public class ShiftsDto : EntityDto
    {
		public DateTime? ScheduledStart { get; set; }

		public DateTime? ScheduledEnd { get; set; }

		public string Name { get; set; }


		 public int JobsId { get; set; }

		 
    }
}