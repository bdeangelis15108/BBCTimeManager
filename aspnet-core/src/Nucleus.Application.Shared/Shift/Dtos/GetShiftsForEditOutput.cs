using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.Shift.Dtos
{
    public class GetShiftsForEditOutput
    {
		public CreateOrEditShiftsDto Shifts { get; set; }

		public string JobsName { get; set;}


    }
}