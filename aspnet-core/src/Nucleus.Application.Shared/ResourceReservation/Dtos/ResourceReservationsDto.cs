
using System;
using Abp.Application.Services.Dto;

namespace Nucleus.ResourceReservation.Dtos
{
    public class ResourceReservationsDto : EntityDto
    {
		public DateTime? ReservedFrom { get; set; }

		public DateTime? ReservedUntil { get; set; }


		 public long? UserId { get; set; }

		 		 public int? ResourcesId { get; set; }

		 
    }
}