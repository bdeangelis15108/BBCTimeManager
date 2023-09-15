

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.Status.Exporting;
using Nucleus.Status.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.Status
{
	[AbpAuthorize(AppPermissions.Pages_Statuses)]
    public class StatusesAppService : NucleusAppServiceBase, IStatusesAppService
    {
		 private readonly IRepository<Statuses> _statusesRepository;
		 private readonly IStatusesExcelExporter _statusesExcelExporter;
		 

		  public StatusesAppService(IRepository<Statuses> statusesRepository, IStatusesExcelExporter statusesExcelExporter ) 
		  {
			_statusesRepository = statusesRepository;
			_statusesExcelExporter = statusesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetStatusesForViewDto>> GetAll(GetAllStatusesInput input)
         {
			
			var filteredStatuses = _statusesRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.ForwardName.Contains(input.Filter) || e.ReverseName.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(input.IsDefaultFilter > -1,  e => (input.IsDefaultFilter == 1 && e.IsDefault) || (input.IsDefaultFilter == 0 && !e.IsDefault) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.ForwardNameFilter),  e => e.ForwardName == input.ForwardNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ReverseNameFilter),  e => e.ReverseName == input.ReverseNameFilter)
						.WhereIf(input.MinForwardIdFilter != null, e => e.ForwardId >= input.MinForwardIdFilter)
						.WhereIf(input.MaxForwardIdFilter != null, e => e.ForwardId <= input.MaxForwardIdFilter)
						.WhereIf(input.MinReverseIdFilter != null, e => e.ReverseId >= input.MinReverseIdFilter)
						.WhereIf(input.MaxReverseIdFilter != null, e => e.ReverseId <= input.MaxReverseIdFilter);

			var pagedAndFilteredStatuses = filteredStatuses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var statuses = from o in pagedAndFilteredStatuses
                         select new GetStatusesForViewDto() {
							Statuses = new StatusesDto
							{
                                Name = o.Name,
                                IsDefault = o.IsDefault,
                                ForwardName = o.ForwardName,
                                ReverseName = o.ReverseName,
                                ForwardId = o.ForwardId,
                                ReverseId = o.ReverseId,
                                Id = o.Id
							}
						};

            var totalCount = await filteredStatuses.CountAsync();

            return new PagedResultDto<GetStatusesForViewDto>(
                totalCount,
                await statuses.ToListAsync()
            );
         }
		 
		 public async Task<GetStatusesForViewDto> GetStatusesForView(int id)
         {
            var statuses = await _statusesRepository.GetAsync(id);

            var output = new GetStatusesForViewDto { Statuses = ObjectMapper.Map<StatusesDto>(statuses) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Statuses_Edit)]
		 public async Task<GetStatusesForEditOutput> GetStatusesForEdit(EntityDto input)
         {
            var statuses = await _statusesRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetStatusesForEditOutput {Statuses = ObjectMapper.Map<CreateOrEditStatusesDto>(statuses)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditStatusesDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Statuses_Create)]
		 protected virtual async Task Create(CreateOrEditStatusesDto input)
         {
            var statuses = ObjectMapper.Map<Statuses>(input);

			

            await _statusesRepository.InsertAsync(statuses);
         }

		 [AbpAuthorize(AppPermissions.Pages_Statuses_Edit)]
		 protected virtual async Task Update(CreateOrEditStatusesDto input)
         {
            var statuses = await _statusesRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, statuses);
         }

		 [AbpAuthorize(AppPermissions.Pages_Statuses_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _statusesRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetStatusesToExcel(GetAllStatusesForExcelInput input)
         {
			
			var filteredStatuses = _statusesRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.ForwardName.Contains(input.Filter) || e.ReverseName.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(input.IsDefaultFilter > -1,  e => (input.IsDefaultFilter == 1 && e.IsDefault) || (input.IsDefaultFilter == 0 && !e.IsDefault) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.ForwardNameFilter),  e => e.ForwardName == input.ForwardNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ReverseNameFilter),  e => e.ReverseName == input.ReverseNameFilter)
						.WhereIf(input.MinForwardIdFilter != null, e => e.ForwardId >= input.MinForwardIdFilter)
						.WhereIf(input.MaxForwardIdFilter != null, e => e.ForwardId <= input.MaxForwardIdFilter)
						.WhereIf(input.MinReverseIdFilter != null, e => e.ReverseId >= input.MinReverseIdFilter)
						.WhereIf(input.MaxReverseIdFilter != null, e => e.ReverseId <= input.MaxReverseIdFilter);

			var query = (from o in filteredStatuses
                         select new GetStatusesForViewDto() { 
							Statuses = new StatusesDto
							{
                                Name = o.Name,
                                IsDefault = o.IsDefault,
                                ForwardName = o.ForwardName,
                                ReverseName = o.ReverseName,
                                ForwardId = o.ForwardId,
                                ReverseId = o.ReverseId,
                                Id = o.Id
							}
						 });


            var statusesListDtos = await query.ToListAsync();

            return _statusesExcelExporter.ExportToFile(statusesListDtos);
         }


    }
}