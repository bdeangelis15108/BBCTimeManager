using Abp.Application.Services.Dto;
using System;

namespace Nucleus.ResourceReservation.Dtos
{
    public class GetAllResourceReservationsesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public DateTime? MaxReservedFromFilter { get; set; }
		public DateTime? MinReservedFromFilter { get; set; }

		public DateTime? MaxReservedUntilFilter { get; set; }
		public DateTime? MinReservedUntilFilter { get; set; }


		 public string UserNameFilter { get; set; }

		 		 public string ResourcesNameFilter { get; set; }
		public string ResourcesTypeFilter { get; set; }

		public GetAllResourceReservationsesInput()
		{
			MaxResultCount = AppConsts.DefaultPageSize;
		}
	}
}