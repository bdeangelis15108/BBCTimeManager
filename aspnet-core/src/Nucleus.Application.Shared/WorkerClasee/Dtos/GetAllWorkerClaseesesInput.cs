using Abp.Application.Services.Dto;
using System;

namespace Nucleus.WorkerClasee.Dtos
{
    public class GetAllWorkerClaseesesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string CodeFilter { get; set; }

		public string NameFilter { get; set; }



    }
}