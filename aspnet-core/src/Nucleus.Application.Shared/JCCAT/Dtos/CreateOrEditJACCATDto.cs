
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.JCCAT.Dtos
{
    public class CreateOrEditJACCATDto : EntityDto<int?>
    {

		[Range(JACCATConsts.MinSEQUENCEValue, JACCATConsts.MaxSEQUENCEValue)]
		public int SEQUENCE { get; set; }
		
		
		[Required]
		[StringLength(JACCATConsts.MaxJOBNUMLength, MinimumLength = JACCATConsts.MinJOBNUMLength)]
		public string JOBNUM { get; set; }
		
		
		[StringLength(JACCATConsts.MaxPHASENUMLength, MinimumLength = JACCATConsts.MinPHASENUMLength)]
		public string PHASENUM { get; set; }
		
		
		[StringLength(JACCATConsts.MaxCATNUMLength, MinimumLength = JACCATConsts.MinCATNUMLength)]
		public string CATNUM { get; set; }
		
		
		[StringLength(JACCATConsts.MaxNAMELength, MinimumLength = JACCATConsts.MinNAMELength)]
		public string NAME { get; set; }
		
		

    }
}