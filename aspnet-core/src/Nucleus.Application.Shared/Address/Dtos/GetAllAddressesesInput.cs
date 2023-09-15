using Abp.Application.Services.Dto;
using System;

namespace Nucleus.Address.Dtos
{
    public class GetAllAddressesesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string Linne1Filter { get; set; }

		public string Line2Filter { get; set; }

		public string CityFilter { get; set; }

		public string StateFilter { get; set; }

		public string ZipFilter { get; set; }

		public string LanFilter { get; set; }

		public string LatFilter { get; set; }

		public GetAllAddressesesInput()
		{
			MaxResultCount = AppConsts.DefaultPageSize;
		}

	}
}