using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.JobCategory.Dtos
{
    public class GetJobCategoriesForEditOutput
    {
		public CreateOrEditJobCategoriesDto JobCategories { get; set; }


    }
}