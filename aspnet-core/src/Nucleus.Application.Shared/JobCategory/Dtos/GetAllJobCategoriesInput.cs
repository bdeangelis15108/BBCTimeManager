using Abp.Application.Services.Dto;
using System;

namespace Nucleus.JobCategory.Dtos
{
    public class GetAllJobCategoriesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string CodeFilter { get; set; }

		public string NameFilter { get; set; }

        public GetAllJobCategoriesInput()
        {
            MaxResultCount = AppConsts.DefaultPageSize;
        }

    }
}