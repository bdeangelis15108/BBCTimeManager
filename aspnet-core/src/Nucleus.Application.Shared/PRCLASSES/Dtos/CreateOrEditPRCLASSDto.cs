
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.PRCLASSES.Dtos
{
    public class CreateOrEditPRCLASSDto : EntityDto<int?>
    {

		[StringLength(PRCLASSConsts.MaxNAMELength, MinimumLength = PRCLASSConsts.MinNAMELength)]
		public string NAME { get; set; }
		
		
		 public int UNIONNUM { get; set; }
		 
		 		 public int? CLASS { get; set; }
		 
		 
    }
}