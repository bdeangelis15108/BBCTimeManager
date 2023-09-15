using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.PREMPLOYEES.Dtos;
using Nucleus.Dto;


namespace Nucleus.PREMPLOYEES
{
    public interface IPREMPLOYEESAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPREMPLOYEEForViewDto>> GetAll(GetAllPREMPLOYEESInput input);

        Task<GetPREMPLOYEEForViewDto> GetPREMPLOYEEForView(int id);

		Task<GetPREMPLOYEEForEditOutput> GetPREMPLOYEEForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditPREMPLOYEEDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetPREMPLOYEESToExcel(GetAllPREMPLOYEESForExcelInput input);

		
    }
}