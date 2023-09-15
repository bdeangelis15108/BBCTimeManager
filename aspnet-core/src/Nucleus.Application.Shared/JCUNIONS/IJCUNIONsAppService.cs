using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.JCUNIONS.Dtos;
using Nucleus.Dto;


namespace Nucleus.JCUNIONS
{
    public interface IJCUNIONsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetJCUNIONForViewDto>> GetAll(GetAllJCUNIONsInput input);

        Task<GetJCUNIONForViewDto> GetJCUNIONForView(int id);

		Task<GetJCUNIONForEditOutput> GetJCUNIONForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditJCUNIONDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetJCUNIONsToExcel(GetAllJCUNIONsForExcelInput input);

		
		Task<PagedResultDto<JCUNIONJACCATLookupTableDto>> GetAllJACCATForLookupTable(GetAllForLookupTableInput input);
		
    }
}