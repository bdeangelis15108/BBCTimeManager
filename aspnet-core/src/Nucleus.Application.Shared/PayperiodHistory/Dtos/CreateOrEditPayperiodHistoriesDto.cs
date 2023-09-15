
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.PayperiodHistory.Dtos
{
    public class CreateOrEditPayperiodHistoriesDto : EntityDto<int?>
    {

		[StringLength(PayperiodHistoriesConsts.MaxperiodLength, MinimumLength = PayperiodHistoriesConsts.MinperiodLength)]
		public string period { get; set; }
		
		
		public bool active { get; set; }
		
		
		 public int? PayPeriodsId { get; set; }
		 
		 
    }
}