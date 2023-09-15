using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.JobUnion.Dtos
{
    public class GetJobUnionsForEditOutput
    {
		public CreateOrEditJobUnionsDto JobUnions { get; set; }

		public string JobsName { get; set;}

		public string UnionsNumber { get; set;}


    }
}