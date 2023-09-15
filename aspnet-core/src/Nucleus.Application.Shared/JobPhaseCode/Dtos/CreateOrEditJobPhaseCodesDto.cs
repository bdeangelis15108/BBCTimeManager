
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.JobPhaseCode.Dtos
{
    public class CreateOrEditJobPhaseCodesDto : EntityDto<int?>
    {

		[StringLength(JobPhaseCodesConsts.MaxCodeLength, MinimumLength = JobPhaseCodesConsts.MinCodeLength)]
		public string Code { get; set; }
		
		
		[StringLength(JobPhaseCodesConsts.MaxNameLength, MinimumLength = JobPhaseCodesConsts.MinNameLength)]
		public string Name { get; set; }
		
		
		 public int JobsId { get; set; }
		 
		 
    }
}