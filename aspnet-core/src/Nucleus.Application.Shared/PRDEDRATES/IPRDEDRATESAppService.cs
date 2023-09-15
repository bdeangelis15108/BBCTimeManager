using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.PRDEDRATES.Dtos;
using Nucleus.Dto;


namespace Nucleus.PRDEDRATES
{
    public interface IPRDEDRATESAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPRDEDRATEForViewDto>> GetAll(GetAllPRDEDRATESInput input);

        Task<GetPRDEDRATEForViewDto> GetPRDEDRATEForView(int id);

		Task<GetPRDEDRATEForEditOutput> GetPRDEDRATEForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditPRDEDRATEDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetPRDEDRATESToExcel(GetAllPRDEDRATESForExcelInput input);

		
		Task<PagedResultDto<PRDEDRATEPRCLASSLookupTableDto>> GetAllPRCLASSForLookupTable(GetAllForLookupTableInput input);
		
    }
}