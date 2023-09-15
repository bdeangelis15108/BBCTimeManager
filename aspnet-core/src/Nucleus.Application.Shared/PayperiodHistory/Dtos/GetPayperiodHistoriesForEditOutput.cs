using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.PayperiodHistory.Dtos
{
    public class GetPayperiodHistoriesForEditOutput
    {
		public CreateOrEditPayperiodHistoriesDto PayperiodHistories { get; set; }

		public string PayPeriodsName { get; set;}


    }
}