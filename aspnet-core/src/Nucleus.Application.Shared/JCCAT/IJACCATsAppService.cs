using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.JCCAT.Dtos;
using Nucleus.Dto;


namespace Nucleus.JCCAT
{
    public interface IJACCATsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetJACCATForViewDto>> GetAll(GetAllJACCATsInput input);

        Task<GetJACCATForViewDto> GetJACCATForView(int id);

		Task<GetJACCATForEditOutput> GetJACCATForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditJACCATDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetJACCATsToExcel(GetAllJACCATsForExcelInput input);

		
    }
}