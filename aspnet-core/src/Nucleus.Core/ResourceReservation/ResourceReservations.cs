using Nucleus.Authorization.Users;
using Nucleus.Resource;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.ResourceReservation
{
	[Table("ResourceReservationses")]
    [Audited]
    public class ResourceReservations : Entity 
    {

		public virtual DateTime? ReservedFrom { get; set; }
		
		public virtual DateTime? ReservedUntil { get; set; }
		

		public virtual long? UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
		public virtual int? ResourcesId { get; set; }
		
        [ForeignKey("ResourcesId")]
		public Resources ResourcesFk { get; set; }
		
    }
}