using Nucleus.Job.Dtos;

namespace Nucleus.JobUnion.Dtos
{
    public class GetJobUnionsForViewDto
    {
		public JobUnionsDto JobUnions { get; set; }

       
		public string JobsName { get; set;}
        public JobsDto Jobs { get; set; }

        public string UnionsNumber { get; set;}


    }
}