using Nucleus.Job;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.Shift.Exporting;
using Nucleus.Shift.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.Shift
{
	[AbpAuthorize(AppPermissions.Pages_Shifts)]
    public class ShiftsAppService : NucleusAppServiceBase, IShiftsAppService
    {
		 private readonly IRepository<Shifts> _shiftsRepository;
		 private readonly IShiftsExcelExporter _shiftsExcelExporter;
		 private readonly IRepository<Jobs,int> _lookup_jobsRepository;
		 

		  public ShiftsAppService(IRepository<Shifts> shiftsRepository, IShiftsExcelExporter shiftsExcelExporter , IRepository<Jobs, int> lookup_jobsRepository) 
		  {
			_shiftsRepository = shiftsRepository;
			_shiftsExcelExporter = shiftsExcelExporter;
			_lookup_jobsRepository = lookup_jobsRepository;
		
		  }

		 public async Task<PagedResultDto<GetShiftsForViewDto>> GetAll(GetAllShiftsInput input)
         {
			
			var filteredShifts = _shiftsRepository.GetAll()
						.Include( e => e.JobsFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(input.MinScheduledStartFilter != null, e => e.ScheduledStart >= input.MinScheduledStartFilter)
						.WhereIf(input.MaxScheduledStartFilter != null, e => e.ScheduledStart <= input.MaxScheduledStartFilter)
						.WhereIf(input.MinScheduledEndFilter != null, e => e.ScheduledEnd >= input.MinScheduledEndFilter)
						.WhereIf(input.MaxScheduledEndFilter != null, e => e.ScheduledEnd <= input.MaxScheduledEndFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.JobsNameFilter), e => e.JobsFk != null && e.JobsFk.Name == input.JobsNameFilter);

			var pagedAndFilteredShifts = filteredShifts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var shifts = from o in pagedAndFilteredShifts
                         join o1 in _lookup_jobsRepository.GetAll() on o.JobsId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetShiftsForViewDto() {
							Shifts = new ShiftsDto
							{
                                ScheduledStart = o.ScheduledStart,
                                ScheduledEnd = o.ScheduledEnd,
                                Name = o.Name,
                                Id = o.Id
							},
                         	JobsName = s1 == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredShifts.CountAsync();

            return new PagedResultDto<GetShiftsForViewDto>(
                totalCount,
                await shifts.ToListAsync()
            );
         }
		 
		 public async Task<GetShiftsForViewDto> GetShiftsForView(int id)
         {
            var shifts = await _shiftsRepository.GetAsync(id);

            var output = new GetShiftsForViewDto { Shifts = ObjectMapper.Map<ShiftsDto>(shifts) };

		    if (output.Shifts.JobsId != null)
            {
                var _lookupJobs = await _lookup_jobsRepository.FirstOrDefaultAsync((int)output.Shifts.JobsId);
                output.JobsName = _lookupJobs.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Shifts_Edit)]
		 public async Task<GetShiftsForEditOutput> GetShiftsForEdit(EntityDto input)
         {
            var shifts = await _shiftsRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetShiftsForEditOutput {Shifts = ObjectMapper.Map<CreateOrEditShiftsDto>(shifts)};

		    if (output.Shifts.JobsId != null)
            {
                var _lookupJobs = await _lookup_jobsRepository.FirstOrDefaultAsync((int)output.Shifts.JobsId);
                output.JobsName = _lookupJobs.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditShiftsDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Shifts_Create)]
		 protected virtual async Task Create(CreateOrEditShiftsDto input)
         {
            var shifts = ObjectMapper.Map<Shifts>(input);

			

            await _shiftsRepository.InsertAsync(shifts);
         }

		 [AbpAuthorize(AppPermissions.Pages_Shifts_Edit)]
		 protected virtual async Task Update(CreateOrEditShiftsDto input)
         {
            var shifts = await _shiftsRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, shifts);
         }

		 [AbpAuthorize(AppPermissions.Pages_Shifts_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _shiftsRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetShiftsToExcel(GetAllShiftsForExcelInput input)
         {
			
			var filteredShifts = _shiftsRepository.GetAll()
						.Include( e => e.JobsFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(input.MinScheduledStartFilter != null, e => e.ScheduledStart >= input.MinScheduledStartFilter)
						.WhereIf(input.MaxScheduledStartFilter != null, e => e.ScheduledStart <= input.MaxScheduledStartFilter)
						.WhereIf(input.MinScheduledEndFilter != null, e => e.ScheduledEnd >= input.MinScheduledEndFilter)
						.WhereIf(input.MaxScheduledEndFilter != null, e => e.ScheduledEnd <= input.MaxScheduledEndFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.JobsNameFilter), e => e.JobsFk != null && e.JobsFk.Name == input.JobsNameFilter);

			var query = (from o in filteredShifts
                         join o1 in _lookup_jobsRepository.GetAll() on o.JobsId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetShiftsForViewDto() { 
							Shifts = new ShiftsDto
							{
                                ScheduledStart = o.ScheduledStart,
                                ScheduledEnd = o.ScheduledEnd,
                                Name = o.Name,
                                Id = o.Id
							},
                         	JobsName = s1 == null ? "" : s1.Name.ToString()
						 });


            var shiftsListDtos = await query.ToListAsync();

            return _shiftsExcelExporter.ExportToFile(shiftsListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Shifts)]
         public async Task<PagedResultDto<ShiftsJobsLookupTableDto>> GetAllJobsForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_jobsRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var jobsList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ShiftsJobsLookupTableDto>();
			foreach(var jobs in jobsList){
				lookupTableDtoList.Add(new ShiftsJobsLookupTableDto
				{
					Id = jobs.Id,
					DisplayName = jobs.Name?.ToString()
				});
			}

            return new PagedResultDto<ShiftsJobsLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}