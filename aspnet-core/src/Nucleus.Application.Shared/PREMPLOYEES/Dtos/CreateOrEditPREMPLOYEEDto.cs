
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.PREMPLOYEES.Dtos
{
    public class CreateOrEditPREMPLOYEEDto : EntityDto<int?>
    {

		[Required]
		[StringLength(PREMPLOYEEConsts.MaxEMPNUMLength, MinimumLength = PREMPLOYEEConsts.MinEMPNUMLength)]
		public string EMPNUM { get; set; }
		
		
		[Required]
		[StringLength(PREMPLOYEEConsts.MaxNAMELength, MinimumLength = PREMPLOYEEConsts.MinNAMELength)]
		public string NAME { get; set; }
		
		
		[StringLength(PREMPLOYEEConsts.MaxUNIONNUMLength, MinimumLength = PREMPLOYEEConsts.MinUNIONNUMLength)]
		public string UNIONNUM { get; set; }
		
		
		[StringLength(PREMPLOYEEConsts.MaxUNIONLOCALLength, MinimumLength = PREMPLOYEEConsts.MinUNIONLOCALLength)]
		public string UNIONLOCAL { get; set; }
		
		
		[Required]
		[StringLength(PREMPLOYEEConsts.MaxCLASSLength, MinimumLength = PREMPLOYEEConsts.MinCLASSLength)]
		public string CLASS { get; set; }
		
		
		[StringLength(PREMPLOYEEConsts.MaxWCOMPNUM1Length, MinimumLength = PREMPLOYEEConsts.MinWCOMPNUM1Length)]
		public string WCOMPNUM1 { get; set; }
		
		
		[Required]
		[StringLength(PREMPLOYEEConsts.MaxLASTNAMELength, MinimumLength = PREMPLOYEEConsts.MinLASTNAMELength)]
		public string LASTNAME { get; set; }
		
		
		[Required]
		[StringLength(PREMPLOYEEConsts.MaxFIRSTNAMELength, MinimumLength = PREMPLOYEEConsts.MinFIRSTNAMELength)]
		public string FIRSTNAME { get; set; }
		
		
		[Required]
		[StringLength(PREMPLOYEEConsts.MaxSTATUSLength, MinimumLength = PREMPLOYEEConsts.MinSTATUSLength)]
		public string STATUS { get; set; }
		
		
		public decimal PAYRATE { get; set; }
		
		

    }
}