using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.EQUIPMENTS.Dtos;
using Nucleus.Dto;


namespace Nucleus.EQUIPMENTS
{
    public interface IEQUIPMENTSAppService : IApplicationService 
    {
        Task<PagedResultDto<GetEQUIPMENTForViewDto>> GetAll(GetAllEQUIPMENTSInput input);

        Task<GetEQUIPMENTForViewDto> GetEQUIPMENTForView(int id);

		Task<GetEQUIPMENTForEditOutput> GetEQUIPMENTForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditEQUIPMENTDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetEQUIPMENTSToExcel(GetAllEQUIPMENTSForExcelInput input);

		
    }
}