using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.ECCOSTS.Dtos
{
    public class GetECCOSTForEditOutput
    {
		public CreateOrEditECCOSTDto ECCOST { get; set; }

		public string EQUIPMENTEQUIPNUM { get; set;}


    }
}