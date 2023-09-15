
using System;
using Abp.Application.Services.Dto;

namespace Nucleus.EmployeeUnion.Dtos
{
    public class EmployeeUnionsDto : EntityDto
    {
		public string LocalNumber { get; set; }


		 public int? UnionsId { get; set; }

		 		 public int? ResourcesId { get; set; }

		 
    }
}