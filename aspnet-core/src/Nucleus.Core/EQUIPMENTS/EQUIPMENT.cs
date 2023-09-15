using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.EQUIPMENTS
{
	[Table("EQUIPMENTS")]
    [Audited]
    public class EQUIPMENT : Entity 
    {

		[StringLength(EQUIPMENTConsts.MaxEQUIPNUMLength, MinimumLength = EQUIPMENTConsts.MinEQUIPNUMLength)]
		public virtual string EQUIPNUM { get; set; }
		

    }
}