using Abp.AutoMapper;
using Nucleus.Organizations.Dto;

namespace Nucleus.Models.Users
{
    [AutoMapFrom(typeof(OrganizationUnitDto))]
    public class OrganizationUnitModel : OrganizationUnitDto
    {
        public bool IsAssigned { get; set; }
    }
}