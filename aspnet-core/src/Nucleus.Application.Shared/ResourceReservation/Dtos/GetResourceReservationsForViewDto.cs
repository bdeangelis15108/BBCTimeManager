namespace Nucleus.ResourceReservation.Dtos
{
    public class GetResourceReservationsForViewDto
    {
		public ResourceReservationsDto ResourceReservations { get; set; }

		public string UserName { get; set;}

		public string ResourcesName { get; set;}


    }
}