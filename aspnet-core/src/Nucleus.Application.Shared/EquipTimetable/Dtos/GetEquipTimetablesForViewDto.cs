namespace Nucleus.EquipTimetable.Dtos
{
    public class GetEquipTimetablesForViewDto
    {
        public EquipTimetablesDto EquipTimetables { get; set; }

        public string PayPeriodsName { get; set; }

        public string ResourcesResourceNumber { get; set; }

        public string JobPhaseCodesName { get; set; }

        public string JobCategoriesName { get; set; }

        public string JobsName { get; set; }
        public decimal CostPerHour { get; set; }
        public string ResourcesResourceName { get; set; }


        public string JobCode { get; set; }
        public string PhaseCode { get; set; }
        public string CategoryCode { get; set; }
    }
}