
using System;
using Abp.Application.Services.Dto;

namespace Nucleus.Timesheet.Dtos
{
    public class TimesheetsDto : EntityDto
    {
		public DateTime? CreatedDate { get; set; }

		public DateTime? SubmitedDate { get; set; }

		public string Name { get; set; }


		 public int? StatusesId { get; set; }

		 
    }
}