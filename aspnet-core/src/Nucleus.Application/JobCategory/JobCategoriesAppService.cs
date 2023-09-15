

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.JobCategory.Exporting;
using Nucleus.JobCategory.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.JobCategory
{
	[AbpAuthorize(AppPermissions.Pages_JobCategories)]
    public class JobCategoriesAppService : NucleusAppServiceBase, IJobCategoriesAppService
    {
		 private readonly IRepository<JobCategories> _jobCategoriesRepository;
		 private readonly IJobCategoriesExcelExporter _jobCategoriesExcelExporter;
		 

		  public JobCategoriesAppService(IRepository<JobCategories> jobCategoriesRepository, IJobCategoriesExcelExporter jobCategoriesExcelExporter ) 
		  {
			_jobCategoriesRepository = jobCategoriesRepository;
			_jobCategoriesExcelExporter = jobCategoriesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetJobCategoriesForViewDto>> GetAll(GetAllJobCategoriesInput input)
         {
			
			var filteredJobCategories = _jobCategoriesRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter);

			var pagedAndFilteredJobCategories = filteredJobCategories
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var jobCategories = from o in pagedAndFilteredJobCategories
                         select new GetJobCategoriesForViewDto() {
							JobCategories = new JobCategoriesDto
							{
                                Code = o.Code,
                                Name = o.Name,
                                Id = o.Id
							}
						};

            var totalCount = await filteredJobCategories.CountAsync();

            return new PagedResultDto<GetJobCategoriesForViewDto>(
                totalCount,
                await jobCategories.ToListAsync()
            );
         }
		 
		 public async Task<GetJobCategoriesForViewDto> GetJobCategoriesForView(int id)
         {
            var jobCategories = await _jobCategoriesRepository.GetAsync(id);

            var output = new GetJobCategoriesForViewDto { JobCategories = ObjectMapper.Map<JobCategoriesDto>(jobCategories) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_JobCategories_Edit)]
		 public async Task<GetJobCategoriesForEditOutput> GetJobCategoriesForEdit(EntityDto input)
         {
            var jobCategories = await _jobCategoriesRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetJobCategoriesForEditOutput {JobCategories = ObjectMapper.Map<CreateOrEditJobCategoriesDto>(jobCategories)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditJobCategoriesDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_JobCategories_Create)]
		 protected virtual async Task Create(CreateOrEditJobCategoriesDto input)
         {
            var jobCategories = ObjectMapper.Map<JobCategories>(input);

			

            await _jobCategoriesRepository.InsertAsync(jobCategories);
         }

		 [AbpAuthorize(AppPermissions.Pages_JobCategories_Edit)]
		 protected virtual async Task Update(CreateOrEditJobCategoriesDto input)
         {
            var jobCategories = await _jobCategoriesRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, jobCategories);
         }

		 [AbpAuthorize(AppPermissions.Pages_JobCategories_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _jobCategoriesRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetJobCategoriesToExcel(GetAllJobCategoriesForExcelInput input)
         {
			
			var filteredJobCategories = _jobCategoriesRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter);

			var query = (from o in filteredJobCategories
                         select new GetJobCategoriesForViewDto() { 
							JobCategories = new JobCategoriesDto
							{
                                Code = o.Code,
                                Name = o.Name,
                                Id = o.Id
							}
						 });


            var jobCategoriesListDtos = await query.ToListAsync();

            return _jobCategoriesExcelExporter.ExportToFile(jobCategoriesListDtos);
         }


    }
}