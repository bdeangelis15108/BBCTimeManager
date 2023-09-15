using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.JCUNIONS.Dtos
{
    public class GetJCUNIONForEditOutput
    {
		public CreateOrEditJCUNIONDto JCUNION { get; set; }

		public string JACCATJOBNUM { get; set;}


    }
}