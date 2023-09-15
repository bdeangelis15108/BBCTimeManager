using Abp.Application.Services.Dto;
using System;

namespace Nucleus.Timetable.Dtos
{
    public class GetAllTimetablesForExcelInput
    {
        public string Filter { get; set; }

        public decimal? MaxDay1Filter { get; set; }
        public decimal? MinDay1Filter { get; set; }

        public decimal? MaxDay2Filter { get; set; }
        public decimal? MinDay2Filter { get; set; }

        public decimal? MaxDay3Filter { get; set; }
        public decimal? MinDay3Filter { get; set; }

        public decimal? MaxDay4Filter { get; set; }
        public decimal? MinDay4Filter { get; set; }

        public decimal? MaxDay5Filter { get; set; }
        public decimal? MinDay5Filter { get; set; }

        public decimal? MaxDay6Filter { get; set; }
        public decimal? MinDay6Filter { get; set; }

        public decimal? MaxDay7Filter { get; set; }
        public decimal? MinDay7Filter { get; set; }

        public decimal? MaxAmountFilter { get; set; }
        public decimal? MinAmountFilter { get; set; }

        public DateTime? MaxCreatedOnFilter { get; set; }
        public DateTime? MinCreatedOnFilter { get; set; }

        public int? IsActiveFilter { get; set; }

        public string CostCodeFilter { get; set; }

        public string PayPeriodsNameFilter { get; set; }

        public string ResourcesNameFilter { get; set; }

        public string UnionPayRatesClassFilter { get; set; }

        public string UnionsNumberFilter { get; set; }

        public string AddressesStateFilter { get; set; }

        public string ExpenseTypesDescriptionFilter { get; set; }

        public string CostTypesNameFilter { get; set; }

        public string AccountsNameFilter { get; set; }

        public string UserNameFilter { get; set; }

        public string PayTypesCodeFilter { get; set; }

        public string WorkerClaseesNameFilter { get; set; }

        public string JobCode { get; set; }
        public int PayPeriodId { get; set; }

    }
}