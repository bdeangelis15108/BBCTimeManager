using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.Status.Dtos
{
    public class GetStatusesForEditOutput
    {
		public CreateOrEditStatusesDto Statuses { get; set; }


    }
}