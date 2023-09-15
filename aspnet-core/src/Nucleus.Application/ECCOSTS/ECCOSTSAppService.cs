using Nucleus.EQUIPMENTS;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.ECCOSTS.Exporting;
using Nucleus.ECCOSTS.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.ECCOSTS
{
	[AbpAuthorize(AppPermissions.Pages_ECCOSTS)]
    public class ECCOSTSAppService : NucleusAppServiceBase, IECCOSTSAppService
    {
		 private readonly IRepository<ECCOST> _eccostRepository;
		 private readonly IECCOSTSExcelExporter _eccostsExcelExporter;
		 private readonly IRepository<EQUIPMENT,int> _lookup_equipmentRepository;
		 

		  public ECCOSTSAppService(IRepository<ECCOST> eccostRepository, IECCOSTSExcelExporter eccostsExcelExporter , IRepository<EQUIPMENT, int> lookup_equipmentRepository) 
		  {
			_eccostRepository = eccostRepository;
			_eccostsExcelExporter = eccostsExcelExporter;
			_lookup_equipmentRepository = lookup_equipmentRepository;
		
		  }

		 public async Task<PagedResultDto<GetECCOSTForViewDto>> GetAll(GetAllECCOSTSInput input)
         {
			
			var filteredECCOSTS = _eccostRepository.GetAll()
						.Include( e => e.EQUIPNUMFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.CODENUM.Contains(input.Filter) || e.ESTHOURLY.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CODENUMFilter),  e => e.CODENUM == input.CODENUMFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ESTHOURLYFilter),  e => e.ESTHOURLY == input.ESTHOURLYFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.EQUIPMENTEQUIPNUMFilter), e => e.EQUIPNUMFk != null && e.EQUIPNUMFk.EQUIPNUM == input.EQUIPMENTEQUIPNUMFilter);

			var pagedAndFilteredECCOSTS = filteredECCOSTS
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var eccosts = from o in pagedAndFilteredECCOSTS
                         join o1 in _lookup_equipmentRepository.GetAll() on o.EQUIPNUM equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetECCOSTForViewDto() {
							ECCOST = new ECCOSTDto
							{
                                CODENUM = o.CODENUM,
                                ESTHOURLY = o.ESTHOURLY,
                                Id = o.Id
							},
                         	EQUIPMENTEQUIPNUM = s1 == null ? "" : s1.EQUIPNUM.ToString()
						};

            var totalCount = await filteredECCOSTS.CountAsync();

            return new PagedResultDto<GetECCOSTForViewDto>(
                totalCount,
                await eccosts.ToListAsync()
            );
         }
		 
		 public async Task<GetECCOSTForViewDto> GetECCOSTForView(int id)
         {
            var eccost = await _eccostRepository.GetAsync(id);

            var output = new GetECCOSTForViewDto { ECCOST = ObjectMapper.Map<ECCOSTDto>(eccost) };

		    if (output.ECCOST.EQUIPNUM != null)
            {
                var _lookupEQUIPMENT = await _lookup_equipmentRepository.FirstOrDefaultAsync((int)output.ECCOST.EQUIPNUM);
                output.EQUIPMENTEQUIPNUM = _lookupEQUIPMENT.EQUIPNUM.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ECCOSTS_Edit)]
		 public async Task<GetECCOSTForEditOutput> GetECCOSTForEdit(EntityDto input)
         {
            var eccost = await _eccostRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetECCOSTForEditOutput {ECCOST = ObjectMapper.Map<CreateOrEditECCOSTDto>(eccost)};

		    if (output.ECCOST.EQUIPNUM != null)
            {
                var _lookupEQUIPMENT = await _lookup_equipmentRepository.FirstOrDefaultAsync((int)output.ECCOST.EQUIPNUM);
                output.EQUIPMENTEQUIPNUM = _lookupEQUIPMENT.EQUIPNUM.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditECCOSTDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ECCOSTS_Create)]
		 protected virtual async Task Create(CreateOrEditECCOSTDto input)
         {
            var eccost = ObjectMapper.Map<ECCOST>(input);

			

            await _eccostRepository.InsertAsync(eccost);
         }

		 [AbpAuthorize(AppPermissions.Pages_ECCOSTS_Edit)]
		 protected virtual async Task Update(CreateOrEditECCOSTDto input)
         {
            var eccost = await _eccostRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, eccost);
         }

		 [AbpAuthorize(AppPermissions.Pages_ECCOSTS_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _eccostRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetECCOSTSToExcel(GetAllECCOSTSForExcelInput input)
         {
			
			var filteredECCOSTS = _eccostRepository.GetAll()
						.Include( e => e.EQUIPNUMFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.CODENUM.Contains(input.Filter) || e.ESTHOURLY.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CODENUMFilter),  e => e.CODENUM == input.CODENUMFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ESTHOURLYFilter),  e => e.ESTHOURLY == input.ESTHOURLYFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.EQUIPMENTEQUIPNUMFilter), e => e.EQUIPNUMFk != null && e.EQUIPNUMFk.EQUIPNUM == input.EQUIPMENTEQUIPNUMFilter);

			var query = (from o in filteredECCOSTS
                         join o1 in _lookup_equipmentRepository.GetAll() on o.EQUIPNUM equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetECCOSTForViewDto() { 
							ECCOST = new ECCOSTDto
							{
                                CODENUM = o.CODENUM,
                                ESTHOURLY = o.ESTHOURLY,
                                Id = o.Id
							},
                         	EQUIPMENTEQUIPNUM = s1 == null ? "" : s1.EQUIPNUM.ToString()
						 });


            var eccostListDtos = await query.ToListAsync();

            return _eccostsExcelExporter.ExportToFile(eccostListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_ECCOSTS)]
         public async Task<PagedResultDto<ECCOSTEQUIPMENTLookupTableDto>> GetAllEQUIPMENTForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_equipmentRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.EQUIPNUM != null && e.EQUIPNUM.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var equipmentList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ECCOSTEQUIPMENTLookupTableDto>();
			foreach(var equipment in equipmentList){
				lookupTableDtoList.Add(new ECCOSTEQUIPMENTLookupTableDto
				{
					Id = equipment.Id,
					DisplayName = equipment.EQUIPNUM?.ToString()
				});
			}

            return new PagedResultDto<ECCOSTEQUIPMENTLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}