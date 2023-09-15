

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.Union.Exporting;
using Nucleus.Union.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.Union
{
	[AbpAuthorize(AppPermissions.Pages_Unions)]
    public class UnionsAppService : NucleusAppServiceBase, IUnionsAppService
    {
		 private readonly IRepository<Unions> _unionsRepository;
		 private readonly IUnionsExcelExporter _unionsExcelExporter;
		 

		  public UnionsAppService(IRepository<Unions> unionsRepository, IUnionsExcelExporter unionsExcelExporter ) 
		  {
			_unionsRepository = unionsRepository;
			_unionsExcelExporter = unionsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetUnionsForViewDto>> GetAll(GetAllUnionsInput input)
         {
			
			var filteredUnions = _unionsRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Number.Contains(input.Filter) || e.LocalNumber.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NumberFilter),  e => e.Number == input.NumberFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LocalNumberFilter),  e => e.LocalNumber == input.LocalNumberFilter);

			var pagedAndFilteredUnions = filteredUnions
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var unions = from o in pagedAndFilteredUnions
                         select new GetUnionsForViewDto() {
							Unions = new UnionsDto
							{
                                Number = o.Number,
                                LocalNumber = o.LocalNumber,
                                Id = o.Id
							}
						};

            var totalCount = await filteredUnions.CountAsync();

            return new PagedResultDto<GetUnionsForViewDto>(
                totalCount,
                await unions.ToListAsync()
            );
         }
		 
		 public async Task<GetUnionsForViewDto> GetUnionsForView(int id)
         {
            var unions = await _unionsRepository.GetAsync(id);

            var output = new GetUnionsForViewDto { Unions = ObjectMapper.Map<UnionsDto>(unions) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Unions_Edit)]
		 public async Task<GetUnionsForEditOutput> GetUnionsForEdit(EntityDto input)
         {
            var unions = await _unionsRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetUnionsForEditOutput {Unions = ObjectMapper.Map<CreateOrEditUnionsDto>(unions)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditUnionsDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Unions_Create)]
		 protected virtual async Task Create(CreateOrEditUnionsDto input)
         {
            var unions = ObjectMapper.Map<Unions>(input);

			

            await _unionsRepository.InsertAsync(unions);
         }

		 [AbpAuthorize(AppPermissions.Pages_Unions_Edit)]
		 protected virtual async Task Update(CreateOrEditUnionsDto input)
         {
            var unions = await _unionsRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, unions);
         }

		 [AbpAuthorize(AppPermissions.Pages_Unions_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _unionsRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetUnionsToExcel(GetAllUnionsForExcelInput input)
         {
			
			var filteredUnions = _unionsRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Number.Contains(input.Filter) || e.LocalNumber.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NumberFilter),  e => e.Number == input.NumberFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LocalNumberFilter),  e => e.LocalNumber == input.LocalNumberFilter);

			var query = (from o in filteredUnions
                         select new GetUnionsForViewDto() { 
							Unions = new UnionsDto
							{
                                Number = o.Number,
                                LocalNumber = o.LocalNumber,
                                Id = o.Id
							}
						 });


            var unionsListDtos = await query.ToListAsync();

            return _unionsExcelExporter.ExportToFile(unionsListDtos);
         }


    }
}