using Abp.Application.Services.Dto;
using System;

namespace Nucleus.JobClass.Dtos
{
    public class GetAllJobClassesesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string CodeFilter { get; set; }

		public string NameFilter { get; set; }

        public GetAllJobClassesesInput()
        {
            MaxResultCount = AppConsts.DefaultPageSize;
        }

    }
}