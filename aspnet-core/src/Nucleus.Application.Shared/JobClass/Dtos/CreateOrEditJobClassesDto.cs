
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.JobClass.Dtos
{
    public class CreateOrEditJobClassesDto : EntityDto<int?>
    {

		[StringLength(JobClassesConsts.MaxCodeLength, MinimumLength = JobClassesConsts.MinCodeLength)]
		public string Code { get; set; }
		
		
		[StringLength(JobClassesConsts.MaxNameLength, MinimumLength = JobClassesConsts.MinNameLength)]
		public string Name { get; set; }
		
		

    }
}