
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.JobCategory.Dtos
{
    public class CreateOrEditJobCategoriesDto : EntityDto<int?>
    {

		[StringLength(JobCategoriesConsts.MaxCodeLength, MinimumLength = JobCategoriesConsts.MinCodeLength)]
		public string Code { get; set; }
		
		
		[StringLength(JobCategoriesConsts.MaxNameLength, MinimumLength = JobCategoriesConsts.MinNameLength)]
		public string Name { get; set; }
		
		

    }
}