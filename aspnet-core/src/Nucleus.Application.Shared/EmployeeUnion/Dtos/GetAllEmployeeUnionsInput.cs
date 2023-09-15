using Abp.Application.Services.Dto;
using System;

namespace Nucleus.EmployeeUnion.Dtos
{
    public class GetAllEmployeeUnionsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string LocalNumberFilter { get; set; }


		 public string UnionsNumberFilter { get; set; }

		 		 public string ResourcesNameFilter { get; set; }

        public GetAllEmployeeUnionsInput()
        {
            MaxResultCount = AppConsts.DefaultPageSize;
        }
    }
}