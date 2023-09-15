using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.Resource.Dtos
{
    public class GetResourcesForEditOutput
    {
		public CreateOrEditResourcesDto Resources { get; set; }


    }
}