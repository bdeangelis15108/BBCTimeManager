
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.JobUnion.Dtos
{
    public class CreateOrEditJobUnionsDto : EntityDto<int?>
    {

		[StringLength(JobUnionsConsts.MaxNumberLength, MinimumLength = JobUnionsConsts.MinNumberLength)]
		public string Number { get; set; }
		
		
		 public int? JobsId { get; set; }
		 
		 		 public int? UnionsId { get; set; }
		 
		 
    }
}