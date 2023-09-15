
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.ResourceWorkerInfo.Dtos
{
    public class CreateOrEditResourceWorkerInfosDto : EntityDto<int?>
    {

		[StringLength(ResourceWorkerInfosConsts.MaxFirstNameLength, MinimumLength = ResourceWorkerInfosConsts.MinFirstNameLength)]
		public string FirstName { get; set; }
		
		
		[StringLength(ResourceWorkerInfosConsts.MaxLastNameLength, MinimumLength = ResourceWorkerInfosConsts.MinLastNameLength)]
		public string LastName { get; set; }
		
		
		public string UnionNumber { get; set; }
		
		
		public string UnionLocal { get; set; }
		
		
		public string Wcomp1 { get; set; }
		
		
		 public int? WorkerClaseesId { get; set; }
		 
		 		 public int? ResourcesId { get; set; }
		 
		 
    }
}