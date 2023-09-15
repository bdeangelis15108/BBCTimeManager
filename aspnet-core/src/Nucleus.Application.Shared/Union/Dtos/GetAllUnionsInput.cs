using Abp.Application.Services.Dto;
using System;

namespace Nucleus.Union.Dtos
{
    public class GetAllUnionsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NumberFilter { get; set; }

		public string LocalNumberFilter { get; set; }

        public GetAllUnionsInput()
        {
            MaxResultCount = AppConsts.DefaultPageSize;
        }

    }
}