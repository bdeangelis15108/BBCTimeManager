
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.JCJOBS.Dtos
{
    public class CreateOrEditJCJOBDto : EntityDto<int?>
    {

		[StringLength(JCJOBConsts.MaxSTATELength, MinimumLength = JCJOBConsts.MinSTATELength)]
		public string STATE { get; set; }
		
		
		[StringLength(JCJOBConsts.MaxLOCALITYLength, MinimumLength = JCJOBConsts.MinLOCALITYLength)]
		public string LOCALITY { get; set; }
		
		
		[StringLength(JCJOBConsts.MaxCLASSLength, MinimumLength = JCJOBConsts.MinCLASSLength)]
		public string CLASS { get; set; }
		
		
		[Range(JCJOBConsts.MinCLOSEDValue, JCJOBConsts.MaxCLOSEDValue)]
		public int CLOSED { get; set; }
		
		
		 public int JOBNUM { get; set; }
		 
		 
    }
}