using Abp.Application.Services.Dto;
using System;

namespace Nucleus.ResourceEquipmentInfo.Dtos
{
    public class GetAllResourceEquipmentInfosesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

        public GetAllResourceEquipmentInfosesInput()
        {
            MaxResultCount = AppConsts.DefaultPageSize;
        }

    }
}