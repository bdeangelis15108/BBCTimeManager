using Nucleus.Job;
using Nucleus.Union;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.JobUnion.Exporting;
using Nucleus.JobUnion.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.JobUnion
{
	[AbpAuthorize(AppPermissions.Pages_JobUnions)]
    public class JobUnionsAppService : NucleusAppServiceBase, IJobUnionsAppService
    {
		 private readonly IRepository<JobUnions> _jobUnionsRepository;
		 private readonly IJobUnionsExcelExporter _jobUnionsExcelExporter;
		 private readonly IRepository<Jobs,int> _lookup_jobsRepository;
		 private readonly IRepository<Unions,int> _lookup_unionsRepository;
		 

		  public JobUnionsAppService(IRepository<JobUnions> jobUnionsRepository, IJobUnionsExcelExporter jobUnionsExcelExporter , IRepository<Jobs, int> lookup_jobsRepository, IRepository<Unions, int> lookup_unionsRepository) 
		  {
			_jobUnionsRepository = jobUnionsRepository;
			_jobUnionsExcelExporter = jobUnionsExcelExporter;
			_lookup_jobsRepository = lookup_jobsRepository;
		_lookup_unionsRepository = lookup_unionsRepository;
		
		  }

		 public async Task<PagedResultDto<GetJobUnionsForViewDto>> GetAll(GetAllJobUnionsInput input)
         {
			
			var filteredJobUnions = _jobUnionsRepository.GetAll()
						.Include( e => e.JobsFk)
						.Include( e => e.UnionsFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Number.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NumberFilter),  e => e.Number == input.NumberFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.JobsNameFilter), e => e.JobsFk != null && e.JobsFk.Name == input.JobsNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UnionsNumberFilter), e => e.UnionsFk != null && e.UnionsFk.Number == input.UnionsNumberFilter);

			var pagedAndFilteredJobUnions = filteredJobUnions
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var jobUnions = from o in pagedAndFilteredJobUnions
                            join o1 in _lookup_jobsRepository.GetAll() on o.JobsId equals o1.Id into j1
                            from s1 in j1.DefaultIfEmpty()

                            join o2 in _lookup_unionsRepository.GetAll() on o.UnionsId equals o2.Id into j2
                            from s2 in j2.DefaultIfEmpty()

                            select new GetJobUnionsForViewDto() {
                                JobUnions = new JobUnionsDto
                                {
                                    Number = o.Number,
                                    Id = o.Id,
							    },
                         	JobsName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                Jobs = new Job.Dtos.JobsDto
                                {
                                    Name=s1.Name,
                                    Code=s1.Code,
                                    StartDate=s1.StartDate,
                                    EndDate=s1.EndDate,
                                    AddressesId=s1.AddressesId,
                                    Status=s1.Status
                                },
                                UnionsNumber = s2 == null || s2.Number == null ? "" : s2.Number.ToString()
						};

            var totalCount = await filteredJobUnions.CountAsync();

            return new PagedResultDto<GetJobUnionsForViewDto>(
                totalCount,
                await jobUnions.ToListAsync()
            );
         }
		 
		 public async Task<GetJobUnionsForViewDto> GetJobUnionsForView(int id)
         {
            var jobUnions = await _jobUnionsRepository.GetAsync(id);

            var output = new GetJobUnionsForViewDto { JobUnions = ObjectMapper.Map<JobUnionsDto>(jobUnions) };

		    if (output.JobUnions.JobsId != null)
            {
                var _lookupJobs = await _lookup_jobsRepository.FirstOrDefaultAsync((int)output.JobUnions.JobsId);
                output.JobsName = _lookupJobs?.Name?.ToString();
            }

		    if (output.JobUnions.UnionsId != null)
            {
                var _lookupUnions = await _lookup_unionsRepository.FirstOrDefaultAsync((int)output.JobUnions.UnionsId);
                output.UnionsNumber = _lookupUnions?.Number?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_JobUnions_Edit)]
		 public async Task<GetJobUnionsForEditOutput> GetJobUnionsForEdit(EntityDto input)
         {
            var jobUnions = await _jobUnionsRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetJobUnionsForEditOutput {JobUnions = ObjectMapper.Map<CreateOrEditJobUnionsDto>(jobUnions)};

		    if (output.JobUnions.JobsId != null)
            {
                var _lookupJobs = await _lookup_jobsRepository.FirstOrDefaultAsync((int)output.JobUnions.JobsId);
                output.JobsName = _lookupJobs?.Name?.ToString();
            }

		    if (output.JobUnions.UnionsId != null)
            {
                var _lookupUnions = await _lookup_unionsRepository.FirstOrDefaultAsync((int)output.JobUnions.UnionsId);
                output.UnionsNumber = _lookupUnions?.Number?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditJobUnionsDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_JobUnions_Create)]
		 protected virtual async Task Create(CreateOrEditJobUnionsDto input)
         {
            var jobUnions = ObjectMapper.Map<JobUnions>(input);

			

            await _jobUnionsRepository.InsertAsync(jobUnions);
         }

		 [AbpAuthorize(AppPermissions.Pages_JobUnions_Edit)]
		 protected virtual async Task Update(CreateOrEditJobUnionsDto input)
         {
            var jobUnions = await _jobUnionsRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, jobUnions);
         }

		 [AbpAuthorize(AppPermissions.Pages_JobUnions_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _jobUnionsRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetJobUnionsToExcel(GetAllJobUnionsForExcelInput input)
         {
			
			var filteredJobUnions = _jobUnionsRepository.GetAll()
						.Include( e => e.JobsFk)
						.Include( e => e.UnionsFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Number.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NumberFilter),  e => e.Number == input.NumberFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.JobsNameFilter), e => e.JobsFk != null && e.JobsFk.Name == input.JobsNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UnionsNumberFilter), e => e.UnionsFk != null && e.UnionsFk.Number == input.UnionsNumberFilter);

			var query = (from o in filteredJobUnions
                         join o1 in _lookup_jobsRepository.GetAll() on o.JobsId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_unionsRepository.GetAll() on o.UnionsId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetJobUnionsForViewDto() { 
							JobUnions = new JobUnionsDto
							{
                                Number = o.Number,
                                Id = o.Id
							},
                         	JobsName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                         	UnionsNumber = s2 == null || s2.Number == null ? "" : s2.Number.ToString()
						 });


            var jobUnionsListDtos = await query.ToListAsync();

            return _jobUnionsExcelExporter.ExportToFile(jobUnionsListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_JobUnions)]
         public async Task<PagedResultDto<JobUnionsJobsLookupTableDto>> GetAllJobsForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_jobsRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var jobsList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<JobUnionsJobsLookupTableDto>();
			foreach(var jobs in jobsList){
				lookupTableDtoList.Add(new JobUnionsJobsLookupTableDto
				{
					Id = jobs.Id,
					DisplayName = jobs.Name?.ToString()
				});
			}

            return new PagedResultDto<JobUnionsJobsLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_JobUnions)]
         public async Task<PagedResultDto<JobUnionsUnionsLookupTableDto>> GetAllUnionsForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_unionsRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Number != null && e.Number.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var unionsList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<JobUnionsUnionsLookupTableDto>();
			foreach(var unions in unionsList){
				lookupTableDtoList.Add(new JobUnionsUnionsLookupTableDto
				{
					Id = unions.Id,
					DisplayName = unions.Number?.ToString()
				});
			}

            return new PagedResultDto<JobUnionsUnionsLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}