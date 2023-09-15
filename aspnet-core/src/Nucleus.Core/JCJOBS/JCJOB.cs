using Nucleus.JCCAT;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.JCJOBS
{
	[Table("JCJOBs")]
    [Audited]
    public class JCJOB : Entity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[StringLength(JCJOBConsts.MaxSTATELength, MinimumLength = JCJOBConsts.MinSTATELength)]
		public virtual string STATE { get; set; }
		
		[StringLength(JCJOBConsts.MaxLOCALITYLength, MinimumLength = JCJOBConsts.MinLOCALITYLength)]
		public virtual string LOCALITY { get; set; }
		
		[StringLength(JCJOBConsts.MaxCLASSLength, MinimumLength = JCJOBConsts.MinCLASSLength)]
		public virtual string CLASS { get; set; }
		
		[Range(JCJOBConsts.MinCLOSEDValue, JCJOBConsts.MaxCLOSEDValue)]
		public virtual int CLOSED { get; set; }
		

		public virtual int JOBNUM { get; set; }
		
        [ForeignKey("JOBNUM")]
		public JACCAT JOBNFk { get; set; }
		
    }
}