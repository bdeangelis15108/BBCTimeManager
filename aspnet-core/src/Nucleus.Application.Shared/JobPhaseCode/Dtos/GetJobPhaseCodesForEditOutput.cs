using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.JobPhaseCode.Dtos
{
    public class GetJobPhaseCodesForEditOutput
    {
		public CreateOrEditJobPhaseCodesDto JobPhaseCodes { get; set; }

		public string JobsName { get; set;}


    }
}