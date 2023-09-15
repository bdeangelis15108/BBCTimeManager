using Nucleus.Job;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.JobPhaseCode.Exporting;
using Nucleus.JobPhaseCode.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.JobPhaseCode
{
	[AbpAuthorize(AppPermissions.Pages_JobPhaseCodes)]
    public class JobPhaseCodesAppService : NucleusAppServiceBase, IJobPhaseCodesAppService
    {
		 private readonly IRepository<JobPhaseCodes> _jobPhaseCodesRepository;
		 private readonly IJobPhaseCodesExcelExporter _jobPhaseCodesExcelExporter;
		 private readonly IRepository<Jobs,int> _lookup_jobsRepository;
		 

		  public JobPhaseCodesAppService(IRepository<JobPhaseCodes> jobPhaseCodesRepository, IJobPhaseCodesExcelExporter jobPhaseCodesExcelExporter , IRepository<Jobs, int> lookup_jobsRepository) 
		  {
			_jobPhaseCodesRepository = jobPhaseCodesRepository;
			_jobPhaseCodesExcelExporter = jobPhaseCodesExcelExporter;
			_lookup_jobsRepository = lookup_jobsRepository;
		
		  }

		 public async Task<PagedResultDto<GetJobPhaseCodesForViewDto>> GetAll(GetAllJobPhaseCodesInput input)
         {
			
			var filteredJobPhaseCodes = _jobPhaseCodesRepository.GetAll()
						.Include( e => e.JobsFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.JobsNameFilter), e => e.JobsFk != null && e.JobsFk.Name == input.JobsNameFilter);

			var pagedAndFilteredJobPhaseCodes = filteredJobPhaseCodes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var jobPhaseCodes = from o in pagedAndFilteredJobPhaseCodes
                         join o1 in _lookup_jobsRepository.GetAll() on o.JobsId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetJobPhaseCodesForViewDto() {
							JobPhaseCodes = new JobPhaseCodesDto
							{
                                Code = o.Code,
                                Name = o.Name,
                                Id = o.Id
							},
                         	JobsName = s1 == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredJobPhaseCodes.CountAsync();

            return new PagedResultDto<GetJobPhaseCodesForViewDto>(
                totalCount,
                await jobPhaseCodes.ToListAsync()
            );
         }
		 
		 public async Task<GetJobPhaseCodesForViewDto> GetJobPhaseCodesForView(int id)
         {
            var jobPhaseCodes = await _jobPhaseCodesRepository.GetAsync(id);

            var output = new GetJobPhaseCodesForViewDto { JobPhaseCodes = ObjectMapper.Map<JobPhaseCodesDto>(jobPhaseCodes) };

		    if (output.JobPhaseCodes.JobsId != null)
            {
                var _lookupJobs = await _lookup_jobsRepository.FirstOrDefaultAsync((int)output.JobPhaseCodes.JobsId);
                output.JobsName = _lookupJobs.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_JobPhaseCodes_Edit)]
		 public async Task<GetJobPhaseCodesForEditOutput> GetJobPhaseCodesForEdit(EntityDto input)
         {
            var jobPhaseCodes = await _jobPhaseCodesRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetJobPhaseCodesForEditOutput {JobPhaseCodes = ObjectMapper.Map<CreateOrEditJobPhaseCodesDto>(jobPhaseCodes)};

		    if (output.JobPhaseCodes.JobsId != null)
            {
                var _lookupJobs = await _lookup_jobsRepository.FirstOrDefaultAsync((int)output.JobPhaseCodes.JobsId);
                output.JobsName = _lookupJobs.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditJobPhaseCodesDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_JobPhaseCodes_Create)]
		 protected virtual async Task Create(CreateOrEditJobPhaseCodesDto input)
         {
            var jobPhaseCodes = ObjectMapper.Map<JobPhaseCodes>(input);

			

            await _jobPhaseCodesRepository.InsertAsync(jobPhaseCodes);
         }

		 [AbpAuthorize(AppPermissions.Pages_JobPhaseCodes_Edit)]
		 protected virtual async Task Update(CreateOrEditJobPhaseCodesDto input)
         {
            var jobPhaseCodes = await _jobPhaseCodesRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, jobPhaseCodes);
         }

		 [AbpAuthorize(AppPermissions.Pages_JobPhaseCodes_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _jobPhaseCodesRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetJobPhaseCodesToExcel(GetAllJobPhaseCodesForExcelInput input)
         {
			
			var filteredJobPhaseCodes = _jobPhaseCodesRepository.GetAll()
						.Include( e => e.JobsFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.JobsNameFilter), e => e.JobsFk != null && e.JobsFk.Name == input.JobsNameFilter);

			var query = (from o in filteredJobPhaseCodes
                         join o1 in _lookup_jobsRepository.GetAll() on o.JobsId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetJobPhaseCodesForViewDto() { 
							JobPhaseCodes = new JobPhaseCodesDto
							{
                                Code = o.Code,
                                Name = o.Name,
                                Id = o.Id
							},
                         	JobsName = s1 == null ? "" : s1.Name.ToString()
						 });


            var jobPhaseCodesListDtos = await query.ToListAsync();

            return _jobPhaseCodesExcelExporter.ExportToFile(jobPhaseCodesListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_JobPhaseCodes)]
         public async Task<PagedResultDto<JobPhaseCodesJobsLookupTableDto>> GetAllJobsForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_jobsRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var jobsList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<JobPhaseCodesJobsLookupTableDto>();
			foreach(var jobs in jobsList){
				lookupTableDtoList.Add(new JobPhaseCodesJobsLookupTableDto
				{
					Id = jobs.Id,
					DisplayName = jobs.Name?.ToString()
				});
			}

            return new PagedResultDto<JobPhaseCodesJobsLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}