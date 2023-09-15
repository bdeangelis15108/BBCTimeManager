using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.Timesheet.Dtos
{
    public class GetTimesheetsForEditOutput
    {
		public CreateOrEditTimesheetsDto Timesheets { get; set; }

		public string StatusesName { get; set;}


    }
}