using Abp.Application.Services.Dto;
using System;

namespace Nucleus.EQUIPMENTS.Dtos
{
    public class GetAllEQUIPMENTSInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string EQUIPNUMFilter { get; set; }


        public GetAllEQUIPMENTSInput()
        {
            MaxResultCount = AppConsts.DefaultPageSize;
        }
    }
}