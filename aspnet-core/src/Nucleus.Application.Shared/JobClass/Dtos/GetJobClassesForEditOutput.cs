using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.JobClass.Dtos
{
    public class GetJobClassesForEditOutput
    {
		public CreateOrEditJobClassesDto JobClasses { get; set; }


    }
}