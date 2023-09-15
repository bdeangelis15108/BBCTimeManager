using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.Status
{
	[Table("Statuses")]
    [Audited]
    public class Statuses : Entity 
    {

		[StringLength(StatusesConsts.MaxNameLength, MinimumLength = StatusesConsts.MinNameLength)]
		public virtual string Name { get; set; }
		
		public virtual bool IsDefault { get; set; }
		
		[StringLength(StatusesConsts.MaxForwardNameLength, MinimumLength = StatusesConsts.MinForwardNameLength)]
		public virtual string ForwardName { get; set; }
		
		[StringLength(StatusesConsts.MaxReverseNameLength, MinimumLength = StatusesConsts.MinReverseNameLength)]
		public virtual string ReverseName { get; set; }
		
		public virtual int? ForwardId { get; set; }
		
		public virtual int? ReverseId { get; set; }
		

    }
}