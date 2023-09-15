using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.JCCAT
{
	[Table("JACCATs")]
    [Audited]
    public class JACCAT : Entity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Range(JACCATConsts.MinSEQUENCEValue, JACCATConsts.MaxSEQUENCEValue)]
		public virtual int SEQUENCE { get; set; }
		
		[Required]
		[StringLength(JACCATConsts.MaxJOBNUMLength, MinimumLength = JACCATConsts.MinJOBNUMLength)]
		public virtual string JOBNUM { get; set; }
		
		[StringLength(JACCATConsts.MaxPHASENUMLength, MinimumLength = JACCATConsts.MinPHASENUMLength)]
		public virtual string PHASENUM { get; set; }
		
		[StringLength(JACCATConsts.MaxCATNUMLength, MinimumLength = JACCATConsts.MinCATNUMLength)]
		public virtual string CATNUM { get; set; }
		
		[StringLength(JACCATConsts.MaxNAMELength, MinimumLength = JACCATConsts.MinNAMELength)]
		public virtual string NAME { get; set; }
		

    }
}