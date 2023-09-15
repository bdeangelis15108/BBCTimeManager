

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.JobClass.Exporting;
using Nucleus.JobClass.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.JobClass
{
	[AbpAuthorize(AppPermissions.Pages_JobClasseses)]
    public class JobClassesesAppService : NucleusAppServiceBase, IJobClassesesAppService
    {
		 private readonly IRepository<JobClasses> _jobClassesRepository;
		 private readonly IJobClassesesExcelExporter _jobClassesesExcelExporter;
		 

		  public JobClassesesAppService(IRepository<JobClasses> jobClassesRepository, IJobClassesesExcelExporter jobClassesesExcelExporter ) 
		  {
			_jobClassesRepository = jobClassesRepository;
			_jobClassesesExcelExporter = jobClassesesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetJobClassesForViewDto>> GetAll(GetAllJobClassesesInput input)
         {
			
			var filteredJobClasseses = _jobClassesRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter);

			var pagedAndFilteredJobClasseses = filteredJobClasseses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var jobClasseses = from o in pagedAndFilteredJobClasseses
                         select new GetJobClassesForViewDto() {
							JobClasses = new JobClassesDto
							{
                                Code = o.Code,
                                Name = o.Name,
                                Id = o.Id
							}
						};

            var totalCount = await filteredJobClasseses.CountAsync();

            return new PagedResultDto<GetJobClassesForViewDto>(
                totalCount,
                await jobClasseses.ToListAsync()
            );
         }
		 
		 public async Task<GetJobClassesForViewDto> GetJobClassesForView(int id)
         {
            var jobClasses = await _jobClassesRepository.GetAsync(id);

            var output = new GetJobClassesForViewDto { JobClasses = ObjectMapper.Map<JobClassesDto>(jobClasses) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_JobClasseses_Edit)]
		 public async Task<GetJobClassesForEditOutput> GetJobClassesForEdit(EntityDto input)
         {
            var jobClasses = await _jobClassesRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetJobClassesForEditOutput {JobClasses = ObjectMapper.Map<CreateOrEditJobClassesDto>(jobClasses)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditJobClassesDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_JobClasseses_Create)]
		 protected virtual async Task Create(CreateOrEditJobClassesDto input)
         {
            var jobClasses = ObjectMapper.Map<JobClasses>(input);

			

            await _jobClassesRepository.InsertAsync(jobClasses);
         }

		 [AbpAuthorize(AppPermissions.Pages_JobClasseses_Edit)]
		 protected virtual async Task Update(CreateOrEditJobClassesDto input)
         {
            var jobClasses = await _jobClassesRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, jobClasses);
         }

		 [AbpAuthorize(AppPermissions.Pages_JobClasseses_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _jobClassesRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetJobClassesesToExcel(GetAllJobClassesesForExcelInput input)
         {
			
			var filteredJobClasseses = _jobClassesRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter);

			var query = (from o in filteredJobClasseses
                         select new GetJobClassesForViewDto() { 
							JobClasses = new JobClassesDto
							{
                                Code = o.Code,
                                Name = o.Name,
                                Id = o.Id
							}
						 });


            var jobClassesListDtos = await query.ToListAsync();

            return _jobClassesesExcelExporter.ExportToFile(jobClassesListDtos);
         }


    }
}