
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.JCUNIONS.Dtos
{
    public class CreateOrEditJCUNIONDto : EntityDto<int?>
    {

		[Required]
		[StringLength(JCUNIONConsts.MaxUNIONNUMLength, MinimumLength = JCUNIONConsts.MinUNIONNUMLength)]
		public string UNIONNUM { get; set; }
		
		
		[StringLength(JCUNIONConsts.MaxUNIONLOCALLength, MinimumLength = JCUNIONConsts.MinUNIONLOCALLength)]
		public string UNIONLOCAL { get; set; }
		
		
		 public int JOBNUM { get; set; }
		 
		 
    }
}