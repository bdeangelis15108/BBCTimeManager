using Nucleus.Union;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.UnionPayRate
{
	[Table("UnionPayRates")]
    [Audited]
    public class UnionPayRates : Entity 
    {

		[StringLength(UnionPayRatesConsts.MaxClassLength, MinimumLength = UnionPayRatesConsts.MinClassLength)]
		public virtual string Class { get; set; }
		
		[StringLength(UnionPayRatesConsts.MaxDedtypeLength, MinimumLength = UnionPayRatesConsts.MinDedtypeLength)]
		public virtual string Dedtype { get; set; }
		
		[Range(UnionPayRatesConsts.MinPerhourValue, UnionPayRatesConsts.MaxPerhourValue)]
		public virtual decimal Perhour { get; set; }
		

		public virtual int? UnionsId { get; set; }
		
        [ForeignKey("UnionsId")]
		public Unions UnionsFk { get; set; }
		
    }
}