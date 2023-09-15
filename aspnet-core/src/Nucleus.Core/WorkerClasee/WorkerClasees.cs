using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.WorkerClasee
{
	[Table("WorkerClaseeses")]
    [Audited]
    public class WorkerClasees : Entity 
    {

		[StringLength(WorkerClaseesConsts.MaxCodeLength, MinimumLength = WorkerClaseesConsts.MinCodeLength)]
		public virtual string Code { get; set; }
		
		[StringLength(WorkerClaseesConsts.MaxNameLength, MinimumLength = WorkerClaseesConsts.MinNameLength)]
		public virtual string Name { get; set; }
		

    }
}