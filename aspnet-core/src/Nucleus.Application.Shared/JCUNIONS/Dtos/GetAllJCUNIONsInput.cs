using Abp.Application.Services.Dto;
using System;

namespace Nucleus.JCUNIONS.Dtos
{
    public class GetAllJCUNIONsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string UNIONNUMFilter { get; set; }

		public string UNIONLOCALFilter { get; set; }


		 public string JACCATJOBNUMFilter { get; set; }

        public GetAllJCUNIONsInput()
        {
            MaxResultCount = AppConsts.DefaultPageSize;
        }
    }
}