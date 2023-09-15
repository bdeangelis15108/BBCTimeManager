using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.JCJOBS.Dtos;
using Nucleus.Dto;


namespace Nucleus.JCJOBS
{
    public interface IJCJOBsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetJCJOBForViewDto>> GetAll(GetAllJCJOBsInput input);

        Task<GetJCJOBForViewDto> GetJCJOBForView(int id);

		Task<GetJCJOBForEditOutput> GetJCJOBForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditJCJOBDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetJCJOBsToExcel(GetAllJCJOBsForExcelInput input);

		
		Task<PagedResultDto<JCJOBJACCATLookupTableDto>> GetAllJACCATForLookupTable(GetAllForLookupTableInput input);
		
    }
}