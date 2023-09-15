
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.Job.Dtos
{
    public class CreateOrEditJobsDto : EntityDto<int?>
    {

		[StringLength(JobsConsts.MaxCodeLength, MinimumLength = JobsConsts.MinCodeLength)]
		public string Code { get; set; }
		
		
		[StringLength(JobsConsts.MaxNameLength, MinimumLength = JobsConsts.MinNameLength)]
		public string Name { get; set; }
		
		
		public DateTime? StartDate { get; set; }
		
		
		public DateTime? EndDate { get; set; }
		
		
		public int Status { get; set; }
		
		
		 public int? AddressesId { get; set; }
		 
		 		 public int? JobClassesId { get; set; }
		 
		 
    }
}