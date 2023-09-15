using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.PRCLASSES.Dtos;
using Nucleus.Dto;


namespace Nucleus.PRCLASSES
{
    public interface IPRCLASSsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPRCLASSForViewDto>> GetAll(GetAllPRCLASSsInput input);

        Task<GetPRCLASSForViewDto> GetPRCLASSForView(int id);

		Task<GetPRCLASSForEditOutput> GetPRCLASSForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditPRCLASSDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetPRCLASSsToExcel(GetAllPRCLASSsForExcelInput input);

		
		Task<PagedResultDto<PRCLASSJCUNIONLookupTableDto>> GetAllJCUNIONForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<PRCLASSPREMPLOYEELookupTableDto>> GetAllPREMPLOYEEForLookupTable(GetAllForLookupTableInput input);
		
    }
}