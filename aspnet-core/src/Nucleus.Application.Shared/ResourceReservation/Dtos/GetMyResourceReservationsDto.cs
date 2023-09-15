
using System;
using Abp.Application.Services.Dto;
using Nucleus.Resource.Dtos;

namespace Nucleus.ResourceReservation.Dtos
{
    public class GetMyResourceReservationsDto : EntityDto
    {
		public DateTime? ReservedFrom { get; set; }

		public DateTime? ReservedUntil { get; set; }


		 public long? UserId { get; set; }

		 public int? ResourcesId { get; set; }

		public ResourcesDto Resources{ get; set; }
		public string UserName { get; set; }
		public string ResourcesName { get; set; }
	}
}