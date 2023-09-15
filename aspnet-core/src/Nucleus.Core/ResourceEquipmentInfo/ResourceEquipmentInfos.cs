using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.ResourceEquipmentInfo
{
	[Table("ResourceEquipmentInfoses")]
    [Audited]
    public class ResourceEquipmentInfos : Entity 
    {

		[StringLength(ResourceEquipmentInfosConsts.MaxNameLength, MinimumLength = ResourceEquipmentInfosConsts.MinNameLength)]
		public virtual string Name { get; set; }
		

    }
}