
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.Timesheet.Dtos
{
    public class CreateOrEditTimesheetsDto : EntityDto<int?>
    {

		public DateTime? CreatedDate { get; set; }
		
		
		public DateTime? SubmitedDate { get; set; }
		
		
		[StringLength(TimesheetsConsts.MaxNameLength, MinimumLength = TimesheetsConsts.MinNameLength)]
		public string Name { get; set; }
		
		
		 public int? StatusesId { get; set; }
		 
		 
    }
}