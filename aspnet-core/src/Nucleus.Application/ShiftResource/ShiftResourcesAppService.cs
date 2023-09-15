using Nucleus.Resource;
using Nucleus.PayType;
using Nucleus.JobPhaseCode;
using Nucleus.JobCategory;
using Nucleus.Timesheet;
using Nucleus.Shift;
using Nucleus.WorkerClasee;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.ShiftResource.Exporting;
using Nucleus.ShiftResource.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using NUglify.Helpers;

namespace Nucleus.ShiftResource
{
	[AbpAuthorize(AppPermissions.Pages_ShiftResources)]
    public class ShiftResourcesAppService : NucleusAppServiceBase, IShiftResourcesAppService
    {
		 private readonly IRepository<ShiftResources> _shiftResourcesRepository;
		 private readonly IShiftResourcesExcelExporter _shiftResourcesExcelExporter;
		 private readonly IRepository<Resources,int> _lookup_resourcesRepository;
		 private readonly IRepository<PayTypes,int> _lookup_payTypesRepository;
		 private readonly IRepository<JobPhaseCodes,int> _lookup_jobPhaseCodesRepository;
		 private readonly IRepository<JobCategories,int> _lookup_jobCategoriesRepository;
		 private readonly IRepository<Timesheets,int> _lookup_timesheetsRepository;
		 private readonly IRepository<Shifts,int> _lookup_shiftsRepository;
		 private readonly IRepository<WorkerClasees,int> _lookup_workerClaseesRepository;
		 

		  public ShiftResourcesAppService(IRepository<ShiftResources> shiftResourcesRepository, IShiftResourcesExcelExporter shiftResourcesExcelExporter , IRepository<Resources, int> lookup_resourcesRepository, IRepository<PayTypes, int> lookup_payTypesRepository, IRepository<JobPhaseCodes, int> lookup_jobPhaseCodesRepository, IRepository<JobCategories, int> lookup_jobCategoriesRepository, IRepository<Timesheets, int> lookup_timesheetsRepository, IRepository<Shifts, int> lookup_shiftsRepository, IRepository<WorkerClasees, int> lookup_workerClaseesRepository) 
		  {
			_shiftResourcesRepository = shiftResourcesRepository;
			_shiftResourcesExcelExporter = shiftResourcesExcelExporter;
			_lookup_resourcesRepository = lookup_resourcesRepository;
		_lookup_payTypesRepository = lookup_payTypesRepository;
		_lookup_jobPhaseCodesRepository = lookup_jobPhaseCodesRepository;
		_lookup_jobCategoriesRepository = lookup_jobCategoriesRepository;
		_lookup_timesheetsRepository = lookup_timesheetsRepository;
		_lookup_shiftsRepository = lookup_shiftsRepository;
		_lookup_workerClaseesRepository = lookup_workerClaseesRepository;
		
		  }

		 public async Task<PagedResultDto<GetShiftResourcesForViewDto>> GetAll(GetAllShiftResourcesInput input)
         {
			
			var filteredShiftResources = _shiftResourcesRepository.GetAll()
						.Include( e => e.ResourcesFk)
						.Include( e => e.PayTypesFk)
						.Include( e => e.JobPhaseCodesFk)
						.Include( e => e.JobCategoriesFk)
						.Include( e => e.TimesheetsFk)
						.Include( e => e.ShiftsFk)
						.Include( e => e.WorkerClaseesFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(input.MinHoursWorkedFilter != null, e => e.HoursWorked >= input.MinHoursWorkedFilter)
						.WhereIf(input.MaxHoursWorkedFilter != null, e => e.HoursWorked <= input.MaxHoursWorkedFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ResourcesNameFilter), e => e.ResourcesFk != null && e.ResourcesFk.Name == input.ResourcesNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PayTypesCodeFilter), e => e.PayTypesFk != null && e.PayTypesFk.Code == input.PayTypesCodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.JobPhaseCodesNameFilter), e => e.JobPhaseCodesFk != null && e.JobPhaseCodesFk.Name == input.JobPhaseCodesNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.JobCategoriesNameFilter), e => e.JobCategoriesFk != null && e.JobCategoriesFk.Name == input.JobCategoriesNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TimesheetsNameFilter), e => e.TimesheetsFk != null && e.TimesheetsFk.Name == input.TimesheetsNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TimesheetsStatusFilter), e => e.TimesheetsFk != null && e.TimesheetsFk.StatusesFk.Name == input.TimesheetsStatusFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ShiftsNameFilter), e => e.ShiftsFk != null && e.ShiftsFk.Name == input.ShiftsNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ResourcesTypeFilter), e => e.ResourcesFk != null && e.ResourcesFk.Type == input.ResourcesTypeFilter)
                         .WhereIf(input.MinCreatedDateFilter != null, e => e.TimesheetsFk.CreatedDate >= input.MinCreatedDateFilter)
                        .WhereIf(input.MaxCreatedDateFilter != null, e => e.TimesheetsFk.CreatedDate <= input.MaxCreatedDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WorkerClaseesNameFilter), e => e.WorkerClaseesFk != null && e.WorkerClaseesFk.Name == input.WorkerClaseesNameFilter)                        ;

			var pagedAndFilteredShiftResources = filteredShiftResources
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var shiftResources = from o in pagedAndFilteredShiftResources
                         join o1 in _lookup_resourcesRepository.GetAll() on o.ResourcesId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_payTypesRepository.GetAll() on o.PayTypesId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_jobPhaseCodesRepository.GetAll() on o.JobPhaseCodesId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         join o4 in _lookup_jobCategoriesRepository.GetAll() on o.JobCategoriesId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()
                         
                         join o5 in _lookup_timesheetsRepository.GetAll() on o.TimesheetsId equals o5.Id into j5
                         from s5 in j5.DefaultIfEmpty()
                         
                         join o6 in _lookup_shiftsRepository.GetAll() on o.ShiftsId equals o6.Id into j6
                         from s6 in j6.DefaultIfEmpty()
                         
                         join o7 in _lookup_workerClaseesRepository.GetAll() on o.WorkerClaseesId equals o7.Id into j7
                         from s7 in j7.DefaultIfEmpty()
                         
                         select new GetShiftResourcesForViewDto() {
							ShiftResources = new ShiftResourcesDto
							{
                                HoursWorked = o.HoursWorked,
                                Name = o.Name,
                                Id = o.Id,
                                ResourcesId=o.ResourcesFk.Id,
                                         TimesheetsId = o.TimesheetsId,
                                         ShiftsId = o.ShiftsId
							},
                         	ResourcesName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                         	PayTypesCode = s2 == null || s2.Code == null ? "" : s2.Code.ToString(),
                         	JobPhaseCodesName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                         	JobCategoriesName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
                         	TimesheetsName = s5 == null || s5.Name == null ? "" : s5.Name.ToString(),
                         	ShiftsName = s6 == null || s6.Name == null ? "" : s6.Name.ToString(),
                         	WorkerClaseesName = s7 == null || s7.Name == null ? "" : s7.Name.ToString(),
                           
                            
						};

            var totalCount = await filteredShiftResources.CountAsync();

            return new PagedResultDto<GetShiftResourcesForViewDto>(
                totalCount,
                await shiftResources.ToListAsync()
            );
         }
		 
		 public async Task<GetShiftResourcesForViewDto> GetShiftResourcesForView(int id)
         {
            var shiftResources = await _shiftResourcesRepository.GetAsync(id);

            var output = new GetShiftResourcesForViewDto { ShiftResources = ObjectMapper.Map<ShiftResourcesDto>(shiftResources) };

		    if (output.ShiftResources.ResourcesId != null)
            {
                var _lookupResources = await _lookup_resourcesRepository.FirstOrDefaultAsync((int)output.ShiftResources.ResourcesId);
                output.ResourcesName = _lookupResources?.Name?.ToString();
            }

		    if (output.ShiftResources.PayTypesId != null)
            {
                var _lookupPayTypes = await _lookup_payTypesRepository.FirstOrDefaultAsync((int)output.ShiftResources.PayTypesId);
                output.PayTypesCode = _lookupPayTypes?.Code?.ToString();
            }

		    if (output.ShiftResources.JobPhaseCodesId != null)
            {
                var _lookupJobPhaseCodes = await _lookup_jobPhaseCodesRepository.FirstOrDefaultAsync((int)output.ShiftResources.JobPhaseCodesId);
                output.JobPhaseCodesName = _lookupJobPhaseCodes?.Name?.ToString();
            }

		    if (output.ShiftResources.JobCategoriesId != null)
            {
                var _lookupJobCategories = await _lookup_jobCategoriesRepository.FirstOrDefaultAsync((int)output.ShiftResources.JobCategoriesId);
                output.JobCategoriesName = _lookupJobCategories?.Name?.ToString();
            }

		    if (output.ShiftResources.TimesheetsId != null)
            {
                var _lookupTimesheets = await _lookup_timesheetsRepository.FirstOrDefaultAsync((int)output.ShiftResources.TimesheetsId);
                output.TimesheetsName = _lookupTimesheets?.Name?.ToString();
            }

		    if (output.ShiftResources.ShiftsId != null)
            {
                var _lookupShifts = await _lookup_shiftsRepository.FirstOrDefaultAsync((int)output.ShiftResources.ShiftsId);
                output.ShiftsName = _lookupShifts?.Name?.ToString();
            }

		    if (output.ShiftResources.WorkerClaseesId != null)
            {
                var _lookupWorkerClasees = await _lookup_workerClaseesRepository.FirstOrDefaultAsync((int)output.ShiftResources.WorkerClaseesId);
                output.WorkerClaseesName = _lookupWorkerClasees?.Name?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ShiftResources_Edit)]
		 public async Task<GetShiftResourcesForEditOutput> GetShiftResourcesForEdit(EntityDto input)
         {
            var shiftResources = await _shiftResourcesRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetShiftResourcesForEditOutput {ShiftResources = ObjectMapper.Map<CreateOrEditShiftResourcesDto>(shiftResources)};

		    if (output.ShiftResources.ResourcesId != null)
            {
                var _lookupResources = await _lookup_resourcesRepository.FirstOrDefaultAsync((int)output.ShiftResources.ResourcesId);
                output.ResourcesName = _lookupResources?.Name?.ToString();
            }

		    if (output.ShiftResources.PayTypesId != null)
            {
                var _lookupPayTypes = await _lookup_payTypesRepository.FirstOrDefaultAsync((int)output.ShiftResources.PayTypesId);
                output.PayTypesCode = _lookupPayTypes?.Code?.ToString();
            }

		    if (output.ShiftResources.JobPhaseCodesId != null)
            {
                var _lookupJobPhaseCodes = await _lookup_jobPhaseCodesRepository.FirstOrDefaultAsync((int)output.ShiftResources.JobPhaseCodesId);
                output.JobPhaseCodesName = _lookupJobPhaseCodes?.Name?.ToString();
            }

		    if (output.ShiftResources.JobCategoriesId != null)
            {
                var _lookupJobCategories = await _lookup_jobCategoriesRepository.FirstOrDefaultAsync((int)output.ShiftResources.JobCategoriesId);
                output.JobCategoriesName = _lookupJobCategories?.Name?.ToString();
            }

		    if (output.ShiftResources.TimesheetsId != null)
            {
                var _lookupTimesheets = await _lookup_timesheetsRepository.FirstOrDefaultAsync((int)output.ShiftResources.TimesheetsId);
                output.TimesheetsName = _lookupTimesheets?.Name?.ToString();
            }

		    if (output.ShiftResources.ShiftsId != null)
            {
                var _lookupShifts = await _lookup_shiftsRepository.FirstOrDefaultAsync((int)output.ShiftResources.ShiftsId);
                output.ShiftsName = _lookupShifts?.Name?.ToString();
            }

		    if (output.ShiftResources.WorkerClaseesId != null)
            {
                var _lookupWorkerClasees = await _lookup_workerClaseesRepository.FirstOrDefaultAsync((int)output.ShiftResources.WorkerClaseesId);
                output.WorkerClaseesName = _lookupWorkerClasees?.Name?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditShiftResourcesDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ShiftResources_Create)]
		 protected virtual async Task Create(CreateOrEditShiftResourcesDto input)
         {
            var shiftResources = ObjectMapper.Map<ShiftResources>(input);

			

            await _shiftResourcesRepository.InsertAsync(shiftResources);
         }

		 [AbpAuthorize(AppPermissions.Pages_ShiftResources_Edit)]
		 protected virtual async Task Update(CreateOrEditShiftResourcesDto input)
         {
            var shiftResources = await _shiftResourcesRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, shiftResources);
         }

		 [AbpAuthorize(AppPermissions.Pages_ShiftResources_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _shiftResourcesRepository.DeleteAsync(input.Id);
        } 

		public async Task<FileDto> GetShiftResourcesToExcel(GetAllShiftResourcesForExcelInput input)
         {
			
			var filteredShiftResources = _shiftResourcesRepository.GetAll()
						.Include( e => e.ResourcesFk)
						.Include( e => e.PayTypesFk)
						.Include( e => e.JobPhaseCodesFk)
						.Include( e => e.JobCategoriesFk)
						.Include( e => e.TimesheetsFk)
						.Include( e => e.ShiftsFk)
						.Include( e => e.WorkerClaseesFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(input.MinHoursWorkedFilter != null, e => e.HoursWorked >= input.MinHoursWorkedFilter)
						.WhereIf(input.MaxHoursWorkedFilter != null, e => e.HoursWorked <= input.MaxHoursWorkedFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ResourcesNameFilter), e => e.ResourcesFk != null && e.ResourcesFk.Name == input.ResourcesNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PayTypesCodeFilter), e => e.PayTypesFk != null && e.PayTypesFk.Code == input.PayTypesCodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.JobPhaseCodesNameFilter), e => e.JobPhaseCodesFk != null && e.JobPhaseCodesFk.Name == input.JobPhaseCodesNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.JobCategoriesNameFilter), e => e.JobCategoriesFk != null && e.JobCategoriesFk.Name == input.JobCategoriesNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TimesheetsNameFilter), e => e.TimesheetsFk != null && e.TimesheetsFk.Name == input.TimesheetsNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ShiftsNameFilter), e => e.ShiftsFk != null && e.ShiftsFk.Name == input.ShiftsNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.WorkerClaseesNameFilter), e => e.WorkerClaseesFk != null && e.WorkerClaseesFk.Name == input.WorkerClaseesNameFilter);

			var query = (from o in filteredShiftResources
                         join o1 in _lookup_resourcesRepository.GetAll() on o.ResourcesId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_payTypesRepository.GetAll() on o.PayTypesId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_jobPhaseCodesRepository.GetAll() on o.JobPhaseCodesId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         join o4 in _lookup_jobCategoriesRepository.GetAll() on o.JobCategoriesId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()
                         
                         join o5 in _lookup_timesheetsRepository.GetAll() on o.TimesheetsId equals o5.Id into j5
                         from s5 in j5.DefaultIfEmpty()
                         
                         join o6 in _lookup_shiftsRepository.GetAll() on o.ShiftsId equals o6.Id into j6
                         from s6 in j6.DefaultIfEmpty()
                         
                         join o7 in _lookup_workerClaseesRepository.GetAll() on o.WorkerClaseesId equals o7.Id into j7
                         from s7 in j7.DefaultIfEmpty()
                         
                         select new GetShiftResourcesForViewDto() { 
							ShiftResources = new ShiftResourcesDto
							{
                                HoursWorked = o.HoursWorked,
                                Name = o.Name,
                                Id = o.Id
							},
                         	ResourcesName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                         	PayTypesCode = s2 == null || s2.Code == null ? "" : s2.Code.ToString(),
                         	JobPhaseCodesName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                         	JobCategoriesName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
                         	TimesheetsName = s5 == null || s5.Name == null ? "" : s5.Name.ToString(),
                         	ShiftsName = s6 == null || s6.Name == null ? "" : s6.Name.ToString(),
                         	WorkerClaseesName = s7 == null || s7.Name == null ? "" : s7.Name.ToString()
						 });


            var shiftResourcesListDtos = await query.ToListAsync();

            return _shiftResourcesExcelExporter.ExportToFile(shiftResourcesListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_ShiftResources)]
         public async Task<PagedResultDto<ShiftResourcesResourcesLookupTableDto>> GetAllResourcesForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_resourcesRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var resourcesList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ShiftResourcesResourcesLookupTableDto>();
			foreach(var resources in resourcesList){
				lookupTableDtoList.Add(new ShiftResourcesResourcesLookupTableDto
				{
					Id = resources.Id,
					DisplayName = resources.Name?.ToString()
				});
			}

            return new PagedResultDto<ShiftResourcesResourcesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_ShiftResources)]
         public async Task<PagedResultDto<ShiftResourcesPayTypesLookupTableDto>> GetAllPayTypesForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_payTypesRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Code != null && e.Code.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var payTypesList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ShiftResourcesPayTypesLookupTableDto>();
			foreach(var payTypes in payTypesList){
				lookupTableDtoList.Add(new ShiftResourcesPayTypesLookupTableDto
				{
					Id = payTypes.Id,
					DisplayName = payTypes.Code?.ToString()
				});
			}

            return new PagedResultDto<ShiftResourcesPayTypesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_ShiftResources)]
         public async Task<PagedResultDto<ShiftResourcesJobPhaseCodesLookupTableDto>> GetAllJobPhaseCodesForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_jobPhaseCodesRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var jobPhaseCodesList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ShiftResourcesJobPhaseCodesLookupTableDto>();
			foreach(var jobPhaseCodes in jobPhaseCodesList){
				lookupTableDtoList.Add(new ShiftResourcesJobPhaseCodesLookupTableDto
				{
					Id = jobPhaseCodes.Id,
					DisplayName = jobPhaseCodes.Name?.ToString()
				});
			}

            return new PagedResultDto<ShiftResourcesJobPhaseCodesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_ShiftResources)]
         public async Task<PagedResultDto<ShiftResourcesJobCategoriesLookupTableDto>> GetAllJobCategoriesForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_jobCategoriesRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var jobCategoriesList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ShiftResourcesJobCategoriesLookupTableDto>();
			foreach(var jobCategories in jobCategoriesList){
				lookupTableDtoList.Add(new ShiftResourcesJobCategoriesLookupTableDto
				{
					Id = jobCategories.Id,
					DisplayName = jobCategories.Name?.ToString()
				});
			}

            return new PagedResultDto<ShiftResourcesJobCategoriesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_ShiftResources)]
         public async Task<PagedResultDto<ShiftResourcesTimesheetsLookupTableDto>> GetAllTimesheetsForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_timesheetsRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var timesheetsList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ShiftResourcesTimesheetsLookupTableDto>();
			foreach(var timesheets in timesheetsList){
				lookupTableDtoList.Add(new ShiftResourcesTimesheetsLookupTableDto
				{
					Id = timesheets.Id,
					DisplayName = timesheets.Name?.ToString()
				});
			}

            return new PagedResultDto<ShiftResourcesTimesheetsLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_ShiftResources)]
         public async Task<PagedResultDto<ShiftResourcesShiftsLookupTableDto>> GetAllShiftsForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_shiftsRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var shiftsList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ShiftResourcesShiftsLookupTableDto>();
			foreach(var shifts in shiftsList){
				lookupTableDtoList.Add(new ShiftResourcesShiftsLookupTableDto
				{
					Id = shifts.Id,
					DisplayName = shifts.Name?.ToString()
				});
			}

            return new PagedResultDto<ShiftResourcesShiftsLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_ShiftResources)]
         public async Task<PagedResultDto<ShiftResourcesWorkerClaseesLookupTableDto>> GetAllWorkerClaseesForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_workerClaseesRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var workerClaseesList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ShiftResourcesWorkerClaseesLookupTableDto>();
			foreach(var workerClasees in workerClaseesList){
				lookupTableDtoList.Add(new ShiftResourcesWorkerClaseesLookupTableDto
				{
					Id = workerClasees.Id,
					DisplayName = workerClasees.Name?.ToString()
				});
			}

            return new PagedResultDto<ShiftResourcesWorkerClaseesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}