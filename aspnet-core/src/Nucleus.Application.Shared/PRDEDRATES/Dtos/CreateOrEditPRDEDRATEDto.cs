
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.PRDEDRATES.Dtos
{
    public class CreateOrEditPRDEDRATEDto : EntityDto<int?>
    {

		[StringLength(PRDEDRATEConsts.MaxUNIONLOCALLength, MinimumLength = PRDEDRATEConsts.MinUNIONLOCALLength)]
		public string UNIONLOCAL { get; set; }
		
		
		[StringLength(PRDEDRATEConsts.MaxCLASSLength, MinimumLength = PRDEDRATEConsts.MinCLASSLength)]
		public string CLASS { get; set; }
		
		
		[Range(PRDEDRATEConsts.MinDEDTYPEValue, PRDEDRATEConsts.MaxDEDTYPEValue)]
		public int DEDTYPE { get; set; }
		
		
		[Range(PRDEDRATEConsts.MinPERHRValue, PRDEDRATEConsts.MaxPERHRValue)]
		public decimal PERHR { get; set; }
		
		
		 public int UNIONNUM { get; set; }
		 
		 
    }
}