using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.EquipTimetable.Dtos
{
    public class GetEquipTimetablesForEditOutput
    {
        public CreateOrEditEquipTimetablesDto EquipTimetables { get; set; }

        public string PayPeriodsName { get; set; }

        public string ResourcesResourceNumber { get; set; }

        public string JobPhaseCodesName { get; set; }

        public string JobCategoriesName { get; set; }

        public string JobsName { get; set; }

    }
}