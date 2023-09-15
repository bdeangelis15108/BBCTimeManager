namespace Nucleus.StatusUpdate.Dtos
{
    public class GetStatusUpdatesForViewDto
    {
		public StatusUpdatesDto StatusUpdates { get; set; }

		public string TimesheetsName { get; set;}

		public string StatusesName { get; set;}

		public string JobsName { get; set;}

		public string UserName { get; set;}


    }
}