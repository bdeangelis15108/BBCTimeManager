using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.ShiftResource.Dtos
{
    public class GetShiftResourcesForEditOutput
    {
		public CreateOrEditShiftResourcesDto ShiftResources { get; set; }

		public string ResourcesName { get; set;}

		public string PayTypesCode { get; set;}

		public string JobPhaseCodesName { get; set;}

		public string JobCategoriesName { get; set;}

		public string TimesheetsName { get; set;}

		public string ShiftsName { get; set;}

		public string WorkerClaseesName { get; set;}


    }
}