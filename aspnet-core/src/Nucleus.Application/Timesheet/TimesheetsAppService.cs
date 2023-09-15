using Nucleus.Status;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.Timesheet.Exporting;
using Nucleus.Timesheet.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.Timesheet
{
	[AbpAuthorize(AppPermissions.Pages_Timesheets)]
    public class TimesheetsAppService : NucleusAppServiceBase, ITimesheetsAppService
    {
		 private readonly IRepository<Timesheets> _timesheetsRepository;
		 private readonly ITimesheetsExcelExporter _timesheetsExcelExporter;
		 private readonly IRepository<Statuses,int> _lookup_statusesRepository;
		 

		  public TimesheetsAppService(IRepository<Timesheets> timesheetsRepository, ITimesheetsExcelExporter timesheetsExcelExporter , IRepository<Statuses, int> lookup_statusesRepository) 
		  {
			_timesheetsRepository = timesheetsRepository;
			_timesheetsExcelExporter = timesheetsExcelExporter;
			_lookup_statusesRepository = lookup_statusesRepository;
		
		  }

		 public async Task<PagedResultDto<GetTimesheetsForViewDto>> GetAll(GetAllTimesheetsInput input)
         {
			
			var filteredTimesheets = _timesheetsRepository.GetAll()
						.Include( e => e.StatusesFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(input.MinCreatedDateFilter != null, e => e.CreatedDate >= input.MinCreatedDateFilter)
						.WhereIf(input.MaxCreatedDateFilter != null, e => e.CreatedDate <= input.MaxCreatedDateFilter)
						.WhereIf(input.MinSubmitedDateFilter != null, e => e.SubmitedDate >= input.MinSubmitedDateFilter)
						.WhereIf(input.MaxSubmitedDateFilter != null, e => e.SubmitedDate <= input.MaxSubmitedDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.StatusesNameFilter), e => e.StatusesFk != null && e.StatusesFk.Name == input.StatusesNameFilter);

			var pagedAndFilteredTimesheets = filteredTimesheets
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var timesheets = from o in pagedAndFilteredTimesheets
                         join o1 in _lookup_statusesRepository.GetAll() on o.StatusesId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetTimesheetsForViewDto() {
							Timesheets = new TimesheetsDto
							{
                                CreatedDate = o.CreatedDate,
                                SubmitedDate = o.SubmitedDate,
                                Name = o.Name,
                                Id = o.Id
							},
                         	StatusesName = s1 == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredTimesheets.CountAsync();

            return new PagedResultDto<GetTimesheetsForViewDto>(
                totalCount,
                await timesheets.ToListAsync()
            );
         }
		 
		 public async Task<GetTimesheetsForViewDto> GetTimesheetsForView(int id)
         {
            var timesheets = await _timesheetsRepository.GetAsync(id);

            var output = new GetTimesheetsForViewDto { Timesheets = ObjectMapper.Map<TimesheetsDto>(timesheets) };

		    if (output.Timesheets.StatusesId != null)
            {
                var _lookupStatuses = await _lookup_statusesRepository.FirstOrDefaultAsync((int)output.Timesheets.StatusesId);
                output.StatusesName = _lookupStatuses.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Timesheets_Edit)]
		 public async Task<GetTimesheetsForEditOutput> GetTimesheetsForEdit(EntityDto input)
         {
            var timesheets = await _timesheetsRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetTimesheetsForEditOutput {Timesheets = ObjectMapper.Map<CreateOrEditTimesheetsDto>(timesheets)};

		    if (output.Timesheets.StatusesId != null)
            {
                var _lookupStatuses = await _lookup_statusesRepository.FirstOrDefaultAsync((int)output.Timesheets.StatusesId);
                output.StatusesName = _lookupStatuses.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditTimesheetsDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Timesheets_Create)]
		 protected virtual async Task Create(CreateOrEditTimesheetsDto input)
         {
            var timesheets = ObjectMapper.Map<Timesheets>(input);

			

            await _timesheetsRepository.InsertAsync(timesheets);
         }

		 [AbpAuthorize(AppPermissions.Pages_Timesheets_Edit)]
		 protected virtual async Task Update(CreateOrEditTimesheetsDto input)
         {
            var timesheets = await _timesheetsRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, timesheets);
         }

		 [AbpAuthorize(AppPermissions.Pages_Timesheets_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _timesheetsRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetTimesheetsToExcel(GetAllTimesheetsForExcelInput input)
         {
			
			var filteredTimesheets = _timesheetsRepository.GetAll()
						.Include( e => e.StatusesFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(input.MinCreatedDateFilter != null, e => e.CreatedDate >= input.MinCreatedDateFilter)
						.WhereIf(input.MaxCreatedDateFilter != null, e => e.CreatedDate <= input.MaxCreatedDateFilter)
						.WhereIf(input.MinSubmitedDateFilter != null, e => e.SubmitedDate >= input.MinSubmitedDateFilter)
						.WhereIf(input.MaxSubmitedDateFilter != null, e => e.SubmitedDate <= input.MaxSubmitedDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.StatusesNameFilter), e => e.StatusesFk != null && e.StatusesFk.Name == input.StatusesNameFilter);

			var query = (from o in filteredTimesheets
                         join o1 in _lookup_statusesRepository.GetAll() on o.StatusesId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetTimesheetsForViewDto() { 
							Timesheets = new TimesheetsDto
							{
                                CreatedDate = o.CreatedDate,
                                SubmitedDate = o.SubmitedDate,
                                Name = o.Name,
                                Id = o.Id
							},
                         	StatusesName = s1 == null ? "" : s1.Name.ToString()
						 });


            var timesheetsListDtos = await query.ToListAsync();

            return _timesheetsExcelExporter.ExportToFile(timesheetsListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Timesheets)]
         public async Task<PagedResultDto<TimesheetsStatusesLookupTableDto>> GetAllStatusesForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_statusesRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var statusesList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<TimesheetsStatusesLookupTableDto>();
			foreach(var statuses in statusesList){
				lookupTableDtoList.Add(new TimesheetsStatusesLookupTableDto
				{
					Id = statuses.Id,
					DisplayName = statuses.Name?.ToString()
				});
			}

            return new PagedResultDto<TimesheetsStatusesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}