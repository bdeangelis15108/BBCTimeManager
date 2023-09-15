using Nucleus.PRCLASSES;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.PRDEDRATES
{
	[Table("PRDEDRATES")]
    [Audited]
    public class PRDEDRATE : Entity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[StringLength(PRDEDRATEConsts.MaxUNIONLOCALLength, MinimumLength = PRDEDRATEConsts.MinUNIONLOCALLength)]
		public virtual string UNIONLOCAL { get; set; }
		
		[StringLength(PRDEDRATEConsts.MaxCLASSLength, MinimumLength = PRDEDRATEConsts.MinCLASSLength)]
		public virtual string CLASS { get; set; }
		
		[Range(PRDEDRATEConsts.MinDEDTYPEValue, PRDEDRATEConsts.MaxDEDTYPEValue)]
		public virtual int DEDTYPE { get; set; }
		
		[Range(PRDEDRATEConsts.MinPERHRValue, PRDEDRATEConsts.MaxPERHRValue)]
		public virtual decimal PERHR { get; set; }
		

		public virtual int UNIONNUM { get; set; }
		
        [ForeignKey("UNIONNUM")]
		public PRCLASS UNIONNFk { get; set; }
		
    }
}