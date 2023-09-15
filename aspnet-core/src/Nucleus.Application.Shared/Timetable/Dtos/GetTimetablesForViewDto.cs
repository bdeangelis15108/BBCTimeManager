namespace Nucleus.Timetable.Dtos
{
    public class GetTimetablesForViewDto
    {
        public TimetablesDto Timetables { get; set; }

        public string PayPeriodsName { get; set; }

        public string ResourcesName { get; set; }

        public string UnionPayRatesClass { get; set; }

        public string UnionsNumber { get; set; }
        public string UnionsLocalNumber { get; set; }

        public string AddressesState { get; set; }

        public string ExpenseTypesDescription { get; set; }

        public string CostTypesName { get; set; }

        public string AccountsName { get; set; }

        public string UserName { get; set; }

        public string PayTypesCode { get; set; }

        public string WorkerClaseesName { get; set; }

        public string WorkerClass { get; set; }
        public decimal? UnionPayRatesPerHour { get; set; }
    }
}