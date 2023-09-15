using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.Job.Dtos
{
    public class GetJobsForEditOutput
    {
		public CreateOrEditJobsDto Jobs { get; set; }

		public string AddressesLinne1 { get; set;}

		public string JobClassesName { get; set;}


    }
}