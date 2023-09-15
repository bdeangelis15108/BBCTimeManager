
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.WorkerClasee.Dtos
{
    public class CreateOrEditWorkerClaseesDto : EntityDto<int?>
    {

		[StringLength(WorkerClaseesConsts.MaxCodeLength, MinimumLength = WorkerClaseesConsts.MinCodeLength)]
		public string Code { get; set; }
		
		
		[StringLength(WorkerClaseesConsts.MaxNameLength, MinimumLength = WorkerClaseesConsts.MinNameLength)]
		public string Name { get; set; }
		
		

    }
}