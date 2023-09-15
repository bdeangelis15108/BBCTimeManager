using Nucleus.EQUIPMENTS;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.ECCOSTS
{
	[Table("ECCOSTS")]
    [Audited]
    public class ECCOST : Entity 
    {

		[StringLength(ECCOSTConsts.MaxCODENUMLength, MinimumLength = ECCOSTConsts.MinCODENUMLength)]
		public virtual string CODENUM { get; set; }
		
		[StringLength(ECCOSTConsts.MaxESTHOURLYLength, MinimumLength = ECCOSTConsts.MinESTHOURLYLength)]
		public virtual string ESTHOURLY { get; set; }
		

		public virtual int EQUIPNUM { get; set; }
		
        [ForeignKey("EQUIPNUM")]
		public EQUIPMENT EQUIPNUMFk { get; set; }
		
    }
}