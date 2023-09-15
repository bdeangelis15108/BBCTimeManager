using Abp.Application.Services.Dto;
using System;

namespace Nucleus.JobPhaseCode.Dtos
{
    public class GetAllJobPhaseCodesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string CodeFilter { get; set; }

		public string NameFilter { get; set; }


		 public string JobsNameFilter { get; set; }

        public GetAllJobPhaseCodesInput()
        {
            MaxResultCount = AppConsts.DefaultPageSize;
        }
    }
}