using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.PayType.Dtos
{
    public class GetPayTypesForEditOutput
    {
		public CreateOrEditPayTypesDto PayTypes { get; set; }


    }
}