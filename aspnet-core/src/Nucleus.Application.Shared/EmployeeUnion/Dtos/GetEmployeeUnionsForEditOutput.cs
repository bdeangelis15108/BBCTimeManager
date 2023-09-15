using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.EmployeeUnion.Dtos
{
    public class GetEmployeeUnionsForEditOutput
    {
		public CreateOrEditEmployeeUnionsDto EmployeeUnions { get; set; }

		public string UnionsNumber { get; set;}

		public string ResourcesName { get; set;}


    }
}