using Nucleus.Union;
using Nucleus.Resource;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.EmployeeUnion
{
	[Table("EmployeeUnions")]
    [Audited]
    public class EmployeeUnions : Entity 
    {

		[StringLength(EmployeeUnionsConsts.MaxLocalNumberLength, MinimumLength = EmployeeUnionsConsts.MinLocalNumberLength)]
		public virtual string LocalNumber { get; set; }
		

		public virtual int? UnionsId { get; set; }
		
        [ForeignKey("UnionsId")]
		public Unions UnionsFk { get; set; }
		
		public virtual int? ResourcesId { get; set; }
		
        [ForeignKey("ResourcesId")]
		public Resources ResourcesFk { get; set; }
		
    }
}