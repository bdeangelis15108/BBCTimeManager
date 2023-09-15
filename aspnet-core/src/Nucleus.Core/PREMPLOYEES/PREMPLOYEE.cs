using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.PREMPLOYEES
{
	[Table("PREMPLOYEES")]
    [Audited]
    public class PREMPLOYEE : Entity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Required]
		[StringLength(PREMPLOYEEConsts.MaxEMPNUMLength, MinimumLength = PREMPLOYEEConsts.MinEMPNUMLength)]
		public virtual string EMPNUM { get; set; }
		
		[Required]
		[StringLength(PREMPLOYEEConsts.MaxNAMELength, MinimumLength = PREMPLOYEEConsts.MinNAMELength)]
		public virtual string NAME { get; set; }
		
		[StringLength(PREMPLOYEEConsts.MaxUNIONNUMLength, MinimumLength = PREMPLOYEEConsts.MinUNIONNUMLength)]
		public virtual string UNIONNUM { get; set; }
		
		[StringLength(PREMPLOYEEConsts.MaxUNIONLOCALLength, MinimumLength = PREMPLOYEEConsts.MinUNIONLOCALLength)]
		public virtual string UNIONLOCAL { get; set; }
		
		[Required]
		[StringLength(PREMPLOYEEConsts.MaxCLASSLength, MinimumLength = PREMPLOYEEConsts.MinCLASSLength)]
		public virtual string CLASS { get; set; }
		
		[StringLength(PREMPLOYEEConsts.MaxWCOMPNUM1Length, MinimumLength = PREMPLOYEEConsts.MinWCOMPNUM1Length)]
		public virtual string WCOMPNUM1 { get; set; }
		
		[Required]
		[StringLength(PREMPLOYEEConsts.MaxLASTNAMELength, MinimumLength = PREMPLOYEEConsts.MinLASTNAMELength)]
		public virtual string LASTNAME { get; set; }
		
		[Required]
		[StringLength(PREMPLOYEEConsts.MaxFIRSTNAMELength, MinimumLength = PREMPLOYEEConsts.MinFIRSTNAMELength)]
		public virtual string FIRSTNAME { get; set; }
		
		[Required]
		[StringLength(PREMPLOYEEConsts.MaxSTATUSLength, MinimumLength = PREMPLOYEEConsts.MinSTATUSLength)]
		public virtual string STATUS { get; set; }
		
		public virtual decimal PAYRATE { get; set; }
		

    }
}