using Nucleus.JCCAT;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.JCUNIONS
{
	[Table("JCUNIONs")]
    [Audited]
    public class JCUNION : Entity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Required]
		[StringLength(JCUNIONConsts.MaxUNIONNUMLength, MinimumLength = JCUNIONConsts.MinUNIONNUMLength)]
		public virtual string UNIONNUM { get; set; }
		
		[StringLength(JCUNIONConsts.MaxUNIONLOCALLength, MinimumLength = JCUNIONConsts.MinUNIONLOCALLength)]
		public virtual string UNIONLOCAL { get; set; }
		

		public virtual int JOBNUM { get; set; }
		
        [ForeignKey("JOBNUM")]
		public JACCAT JOBNFk { get; set; }
		
    }
}