using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.Union
{
	[Table("Unions")]
    [Audited]
    public class Unions : Entity 
    {

		[StringLength(UnionsConsts.MaxNumberLength, MinimumLength = UnionsConsts.MinNumberLength)]
		public virtual string Number { get; set; }
		
		[StringLength(UnionsConsts.MaxLocalNumberLength, MinimumLength = UnionsConsts.MinLocalNumberLength)]
		public virtual string LocalNumber { get; set; }
		

    }
}