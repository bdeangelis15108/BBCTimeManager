using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.JCJOBS.Dtos
{
    public class GetJCJOBForEditOutput
    {
		public CreateOrEditJCJOBDto JCJOB { get; set; }

		public string JACCATJOBNUM { get; set;}


    }
}