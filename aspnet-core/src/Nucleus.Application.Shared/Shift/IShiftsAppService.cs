using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.Shift.Dtos;
using Nucleus.Dto;


namespace Nucleus.Shift
{
    public interface IShiftsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetShiftsForViewDto>> GetAll(GetAllShiftsInput input);

        Task<GetShiftsForViewDto> GetShiftsForView(int id);

		Task<GetShiftsForEditOutput> GetShiftsForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditShiftsDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetShiftsToExcel(GetAllShiftsForExcelInput input);

		
		Task<PagedResultDto<ShiftsJobsLookupTableDto>> GetAllJobsForLookupTable(GetAllForLookupTableInput input);
		
    }
}