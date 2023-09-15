using Abp.Application.Services.Dto;
using System;

namespace Nucleus.PRCLASSES.Dtos
{
    public class GetAllPRCLASSsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NAMEFilter { get; set; }


		 public string JCUNIONUNIONNUMFilter { get; set; }

		 		 public string PREMPLOYEECLASSFilter { get; set; }
        public GetAllPRCLASSsInput()
        {
            MaxResultCount = AppConsts.DefaultPageSize;
        }

    }
}