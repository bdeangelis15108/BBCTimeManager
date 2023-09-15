

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.WorkerClasee.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.WorkerClasee
{
	[AbpAuthorize(AppPermissions.Pages_WorkerClaseeses)]
    public class WorkerClaseesesAppService : NucleusAppServiceBase, IWorkerClaseesesAppService
    {
		 private readonly IRepository<WorkerClasees> _workerClaseesRepository;
		 

		  public WorkerClaseesesAppService(IRepository<WorkerClasees> workerClaseesRepository ) 
		  {
			_workerClaseesRepository = workerClaseesRepository;
			
		  }

		 public async Task<PagedResultDto<GetWorkerClaseesForViewDto>> GetAll(GetAllWorkerClaseesesInput input)
         {
			
			var filteredWorkerClaseeses = _workerClaseesRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter);

			var pagedAndFilteredWorkerClaseeses = filteredWorkerClaseeses
				.OrderBy(input.Sorting ?? "id asc");
               // .PageBy(input);

			var workerClaseeses = from o in pagedAndFilteredWorkerClaseeses
                         select new GetWorkerClaseesForViewDto() {
							WorkerClasees = new WorkerClaseesDto
							{
                                Code = o.Code,
                                Name = o.Name,
                                Id = o.Id
							}
						};

            var totalCount = await filteredWorkerClaseeses.CountAsync();

            return new PagedResultDto<GetWorkerClaseesForViewDto>(
                totalCount,
                await workerClaseeses.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_WorkerClaseeses_Edit)]
		 public async Task<GetWorkerClaseesForEditOutput> GetWorkerClaseesForEdit(EntityDto input)
         {
            var workerClasees = await _workerClaseesRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetWorkerClaseesForEditOutput {WorkerClasees = ObjectMapper.Map<CreateOrEditWorkerClaseesDto>(workerClasees)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditWorkerClaseesDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_WorkerClaseeses_Create)]
		 protected virtual async Task Create(CreateOrEditWorkerClaseesDto input)
         {
            var workerClasees = ObjectMapper.Map<WorkerClasees>(input);


			workerClasees.Id = -1;
            await _workerClaseesRepository.InsertAsync(workerClasees);
         }

		 [AbpAuthorize(AppPermissions.Pages_WorkerClaseeses_Edit)]
		 protected virtual async Task Update(CreateOrEditWorkerClaseesDto input)
         {
            var workerClasees = await _workerClaseesRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, workerClasees);
         }

		 [AbpAuthorize(AppPermissions.Pages_WorkerClaseeses_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _workerClaseesRepository.DeleteAsync(input.Id);
         } 
    }
}