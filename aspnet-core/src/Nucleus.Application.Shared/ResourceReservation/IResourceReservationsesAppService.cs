using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.ResourceReservation.Dtos;
using Nucleus.Dto;


namespace Nucleus.ResourceReservation
{
    public interface IResourceReservationsesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetResourceReservationsForViewDto>> GetAll(GetAllResourceReservationsesInput input);

        Task<GetResourceReservationsForViewDto> GetResourceReservationsForView(int id);

		Task<GetResourceReservationsForEditOutput> GetResourceReservationsForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditResourceReservationsDto input);

		Task Delete(EntityDto input);
		Task DeleteByResourceId(int id);

		Task<FileDto> GetResourceReservationsesToExcel(GetAllResourceReservationsesForExcelInput input);

		
		Task<PagedResultDto<ResourceReservationsUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ResourceReservationsResourcesLookupTableDto>> GetAllResourcesForLookupTable(GetAllForLookupTableInput input);
		Task<PagedResultDto<GetMyResourceReservationsDto>> GetMyReservedResource(GetAllResourceReservationsesInput input);


	}
}