
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.EmployeeUnion.Dtos
{
    public class CreateOrEditEmployeeUnionsDto : EntityDto<int?>
    {

		[StringLength(EmployeeUnionsConsts.MaxLocalNumberLength, MinimumLength = EmployeeUnionsConsts.MinLocalNumberLength)]
		public string LocalNumber { get; set; }
		
		
		 public int? UnionsId { get; set; }
		 
		 		 public int? ResourcesId { get; set; }
		 
		 
    }
}