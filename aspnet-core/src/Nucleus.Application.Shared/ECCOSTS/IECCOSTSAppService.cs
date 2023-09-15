using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.ECCOSTS.Dtos;
using Nucleus.Dto;


namespace Nucleus.ECCOSTS
{
    public interface IECCOSTSAppService : IApplicationService 
    {
        Task<PagedResultDto<GetECCOSTForViewDto>> GetAll(GetAllECCOSTSInput input);

        Task<GetECCOSTForViewDto> GetECCOSTForView(int id);

		Task<GetECCOSTForEditOutput> GetECCOSTForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditECCOSTDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetECCOSTSToExcel(GetAllECCOSTSForExcelInput input);

		
		Task<PagedResultDto<ECCOSTEQUIPMENTLookupTableDto>> GetAllEQUIPMENTForLookupTable(GetAllForLookupTableInput input);
		
    }
}