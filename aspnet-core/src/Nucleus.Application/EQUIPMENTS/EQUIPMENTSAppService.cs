

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.EQUIPMENTS.Exporting;
using Nucleus.EQUIPMENTS.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.EQUIPMENTS
{
	[AbpAuthorize(AppPermissions.Pages_EQUIPMENTS)]
    public class EQUIPMENTSAppService : NucleusAppServiceBase, IEQUIPMENTSAppService
    {
		 private readonly IRepository<EQUIPMENT> _equipmentRepository;
		 private readonly IEQUIPMENTSExcelExporter _equipmentsExcelExporter;
		 

		  public EQUIPMENTSAppService(IRepository<EQUIPMENT> equipmentRepository, IEQUIPMENTSExcelExporter equipmentsExcelExporter ) 
		  {
			_equipmentRepository = equipmentRepository;
			_equipmentsExcelExporter = equipmentsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetEQUIPMENTForViewDto>> GetAll(GetAllEQUIPMENTSInput input)
         {
			
			var filteredEQUIPMENTS = _equipmentRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.EQUIPNUM.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.EQUIPNUMFilter),  e => e.EQUIPNUM == input.EQUIPNUMFilter);

			var pagedAndFilteredEQUIPMENTS = filteredEQUIPMENTS
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var equipments = from o in pagedAndFilteredEQUIPMENTS
                         select new GetEQUIPMENTForViewDto() {
							EQUIPMENT = new EQUIPMENTDto
							{
                                EQUIPNUM = o.EQUIPNUM,
                                Id = o.Id
							}
						};

            var totalCount = await filteredEQUIPMENTS.CountAsync();

            return new PagedResultDto<GetEQUIPMENTForViewDto>(
                totalCount,
                await equipments.ToListAsync()
            );
         }
		 
		 public async Task<GetEQUIPMENTForViewDto> GetEQUIPMENTForView(int id)
         {
            var equipment = await _equipmentRepository.GetAsync(id);

            var output = new GetEQUIPMENTForViewDto { EQUIPMENT = ObjectMapper.Map<EQUIPMENTDto>(equipment) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_EQUIPMENTS_Edit)]
		 public async Task<GetEQUIPMENTForEditOutput> GetEQUIPMENTForEdit(EntityDto input)
         {
            var equipment = await _equipmentRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetEQUIPMENTForEditOutput {EQUIPMENT = ObjectMapper.Map<CreateOrEditEQUIPMENTDto>(equipment)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditEQUIPMENTDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_EQUIPMENTS_Create)]
		 protected virtual async Task Create(CreateOrEditEQUIPMENTDto input)
         {
            var equipment = ObjectMapper.Map<EQUIPMENT>(input);

			

            await _equipmentRepository.InsertAsync(equipment);
         }

		 [AbpAuthorize(AppPermissions.Pages_EQUIPMENTS_Edit)]
		 protected virtual async Task Update(CreateOrEditEQUIPMENTDto input)
         {
            var equipment = await _equipmentRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, equipment);
         }

		 [AbpAuthorize(AppPermissions.Pages_EQUIPMENTS_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _equipmentRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetEQUIPMENTSToExcel(GetAllEQUIPMENTSForExcelInput input)
         {
			
			var filteredEQUIPMENTS = _equipmentRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.EQUIPNUM.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.EQUIPNUMFilter),  e => e.EQUIPNUM == input.EQUIPNUMFilter);

			var query = (from o in filteredEQUIPMENTS
                         select new GetEQUIPMENTForViewDto() { 
							EQUIPMENT = new EQUIPMENTDto
							{
                                EQUIPNUM = o.EQUIPNUM,
                                Id = o.Id
							}
						 });


            var equipmentListDtos = await query.ToListAsync();

            return _equipmentsExcelExporter.ExportToFile(equipmentListDtos);
         }


    }
}