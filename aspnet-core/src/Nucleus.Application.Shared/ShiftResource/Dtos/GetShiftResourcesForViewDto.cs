namespace Nucleus.ShiftResource.Dtos
{
    public class GetShiftResourcesForViewDto
    {
		public ShiftResourcesDto ShiftResources { get; set; }

		public string ResourcesName { get; set;}

		public string PayTypesCode { get; set;}

		public string JobPhaseCodesName { get; set;}

		public string JobCategoriesName { get; set;}

		public string TimesheetsName { get; set;}

		public string ShiftsName { get; set;}

		public string WorkerClaseesName { get; set;}


    }
}