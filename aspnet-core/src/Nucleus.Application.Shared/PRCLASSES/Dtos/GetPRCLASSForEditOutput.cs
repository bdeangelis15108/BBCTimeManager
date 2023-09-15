using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.PRCLASSES.Dtos
{
    public class GetPRCLASSForEditOutput
    {
		public CreateOrEditPRCLASSDto PRCLASS { get; set; }

		public string JCUNIONUNIONNUM { get; set;}

		public string PREMPLOYEECLASS { get; set;}


    }
}