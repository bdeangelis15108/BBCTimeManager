using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.Timetable.Dtos
{
    public class GetTimetablesForEditOutput
    {
        public CreateOrEditTimetablesDto Timetables { get; set; }

        public string PayPeriodsName { get; set; }

        public string ResourcesName { get; set; }

        public string UnionPayRatesClass { get; set; }

        public string UnionsNumber { get; set; }

        public string AddressesState { get; set; }

        public string ExpenseTypesDescription { get; set; }

        public string CostTypesName { get; set; }

        public string AccountsName { get; set; }

        public string UserName { get; set; }

        public string PayTypesCode { get; set; }

        public string WorkerClaseesName { get; set; }

    }
}