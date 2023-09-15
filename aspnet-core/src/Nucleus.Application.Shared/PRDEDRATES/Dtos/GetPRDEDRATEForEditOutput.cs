using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.PRDEDRATES.Dtos
{
    public class GetPRDEDRATEForEditOutput
    {
		public CreateOrEditPRDEDRATEDto PRDEDRATE { get; set; }

		public string PRCLASSUNIONNUM { get; set;}


    }
}