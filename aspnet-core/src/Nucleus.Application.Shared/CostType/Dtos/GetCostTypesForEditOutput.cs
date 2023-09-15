using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.CostType.Dtos
{
    public class GetCostTypesForEditOutput
    {
		public CreateOrEditCostTypesDto CostTypes { get; set; }


    }
}