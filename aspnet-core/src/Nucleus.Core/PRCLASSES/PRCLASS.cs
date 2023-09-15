using Nucleus.JCUNIONS;
using Nucleus.PREMPLOYEES;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.PRCLASSES
{
	[Table("PRCLASSs")]
    [Audited]
    public class PRCLASS : Entity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[StringLength(PRCLASSConsts.MaxNAMELength, MinimumLength = PRCLASSConsts.MinNAMELength)]
		public virtual string NAME { get; set; }
		

		public virtual int UNIONNUM { get; set; }
		
        [ForeignKey("UNIONNUM")]
		public JCUNION UNIONNFk { get; set; }
		
		public virtual int? CLASS { get; set; }
		
        [ForeignKey("CLASS")]
		public PREMPLOYEE CLAFk { get; set; }
		
    }
}