using Nucleus.WorkerClasee;
using Nucleus.Resource;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.ResourceWorkerInfo
{
	[Table("ResourceWorkerInfoses")]
    [Audited]
    public class ResourceWorkerInfos : Entity 
    {

		[StringLength(ResourceWorkerInfosConsts.MaxFirstNameLength, MinimumLength = ResourceWorkerInfosConsts.MinFirstNameLength)]
		public virtual string FirstName { get; set; }
		
		[StringLength(ResourceWorkerInfosConsts.MaxLastNameLength, MinimumLength = ResourceWorkerInfosConsts.MinLastNameLength)]
		public virtual string LastName { get; set; }
		/// <summary>
		/// Number, "IBEW" or "EXEMPT.
		/// </summary>
		public virtual string UnionNumber { get; set; }
		/// <summary>
		/// UnionLocal
		/// </summary>
		public virtual string UnionLocal { get; set; }
		
		public virtual string Wcomp1 { get; set; }
		

		public virtual int? WorkerClaseesId { get; set; }
		
        [ForeignKey("WorkerClaseesId")]
		public WorkerClasees WorkerClaseesFk { get; set; }
		
		public virtual int? ResourcesId { get; set; }
		
        [ForeignKey("ResourcesId")]
		public Resources ResourcesFk { get; set; }
		
    }
}