using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.PayPeriod.Dtos
{
    public class GetPayPeriodsForEditOutput
    {
		public CreateOrEditPayPeriodsDto PayPeriods { get; set; }


    }
}