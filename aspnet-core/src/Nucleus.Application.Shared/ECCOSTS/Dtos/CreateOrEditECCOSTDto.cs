
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.ECCOSTS.Dtos
{
    public class CreateOrEditECCOSTDto : EntityDto<int?>
    {

		[StringLength(ECCOSTConsts.MaxCODENUMLength, MinimumLength = ECCOSTConsts.MinCODENUMLength)]
		public string CODENUM { get; set; }
		
		
		[StringLength(ECCOSTConsts.MaxESTHOURLYLength, MinimumLength = ECCOSTConsts.MinESTHOURLYLength)]
		public string ESTHOURLY { get; set; }
		
		
		 public int EQUIPNUM { get; set; }
		 
		 
    }
}