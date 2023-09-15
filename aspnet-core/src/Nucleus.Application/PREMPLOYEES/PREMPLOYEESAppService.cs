

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.PREMPLOYEES.Exporting;
using Nucleus.PREMPLOYEES.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.PREMPLOYEES
{
	[AbpAuthorize(AppPermissions.Pages_PREMPLOYEES)]
    public class PREMPLOYEESAppService : NucleusAppServiceBase, IPREMPLOYEESAppService
    {
		 private readonly IRepository<PREMPLOYEE> _premployeeRepository;
		 private readonly IPREMPLOYEESExcelExporter _premployeesExcelExporter;
		 

		  public PREMPLOYEESAppService(IRepository<PREMPLOYEE> premployeeRepository, IPREMPLOYEESExcelExporter premployeesExcelExporter ) 
		  {
			_premployeeRepository = premployeeRepository;
			_premployeesExcelExporter = premployeesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetPREMPLOYEEForViewDto>> GetAll(GetAllPREMPLOYEESInput input)
         {
			
			var filteredPREMPLOYEES = _premployeeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.EMPNUM.Contains(input.Filter) || e.NAME.Contains(input.Filter) || e.UNIONNUM.Contains(input.Filter) || e.UNIONLOCAL.Contains(input.Filter) || e.CLASS.Contains(input.Filter) || e.WCOMPNUM1.Contains(input.Filter) || e.LASTNAME.Contains(input.Filter) || e.FIRSTNAME.Contains(input.Filter) || e.STATUS.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.EMPNUMFilter),  e => e.EMPNUM == input.EMPNUMFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NAMEFilter),  e => e.NAME == input.NAMEFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UNIONNUMFilter),  e => e.UNIONNUM == input.UNIONNUMFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UNIONLOCALFilter),  e => e.UNIONLOCAL == input.UNIONLOCALFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CLASSFilter),  e => e.CLASS == input.CLASSFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.WCOMPNUM1Filter),  e => e.WCOMPNUM1 == input.WCOMPNUM1Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LASTNAMEFilter),  e => e.LASTNAME == input.LASTNAMEFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.FIRSTNAMEFilter),  e => e.FIRSTNAME == input.FIRSTNAMEFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.STATUSFilter),  e => e.STATUS == input.STATUSFilter)
						.WhereIf(input.MinPAYRATEFilter != null, e => e.PAYRATE >= input.MinPAYRATEFilter)
						.WhereIf(input.MaxPAYRATEFilter != null, e => e.PAYRATE <= input.MaxPAYRATEFilter);

			var pagedAndFilteredPREMPLOYEES = filteredPREMPLOYEES
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var premployees = from o in pagedAndFilteredPREMPLOYEES
                         select new GetPREMPLOYEEForViewDto() {
							PREMPLOYEE = new PREMPLOYEEDto
							{
                                EMPNUM = o.EMPNUM,
                                NAME = o.NAME,
                                UNIONNUM = o.UNIONNUM,
                                UNIONLOCAL = o.UNIONLOCAL,
                                CLASS = o.CLASS,
                                WCOMPNUM1 = o.WCOMPNUM1,
                                LASTNAME = o.LASTNAME,
                                FIRSTNAME = o.FIRSTNAME,
                                STATUS = o.STATUS,
                                PAYRATE = o.PAYRATE,
                                Id = o.Id
							}
						};

            var totalCount = await filteredPREMPLOYEES.CountAsync();

            return new PagedResultDto<GetPREMPLOYEEForViewDto>(
                totalCount,
                await premployees.ToListAsync()
            );
         }
		 
		 public async Task<GetPREMPLOYEEForViewDto> GetPREMPLOYEEForView(int id)
         {
            var premployee = await _premployeeRepository.GetAsync(id);

            var output = new GetPREMPLOYEEForViewDto { PREMPLOYEE = ObjectMapper.Map<PREMPLOYEEDto>(premployee) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_PREMPLOYEES_Edit)]
		 public async Task<GetPREMPLOYEEForEditOutput> GetPREMPLOYEEForEdit(EntityDto input)
         {
            var premployee = await _premployeeRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetPREMPLOYEEForEditOutput {PREMPLOYEE = ObjectMapper.Map<CreateOrEditPREMPLOYEEDto>(premployee)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditPREMPLOYEEDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_PREMPLOYEES_Create)]
		 protected virtual async Task Create(CreateOrEditPREMPLOYEEDto input)
         {
            var premployee = ObjectMapper.Map<PREMPLOYEE>(input);

			
			if (AbpSession.TenantId != null)
			{
				premployee.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _premployeeRepository.InsertAsync(premployee);
         }

		 [AbpAuthorize(AppPermissions.Pages_PREMPLOYEES_Edit)]
		 protected virtual async Task Update(CreateOrEditPREMPLOYEEDto input)
         {
            var premployee = await _premployeeRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, premployee);
         }

		 [AbpAuthorize(AppPermissions.Pages_PREMPLOYEES_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _premployeeRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetPREMPLOYEESToExcel(GetAllPREMPLOYEESForExcelInput input)
         {
			
			var filteredPREMPLOYEES = _premployeeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.EMPNUM.Contains(input.Filter) || e.NAME.Contains(input.Filter) || e.UNIONNUM.Contains(input.Filter) || e.UNIONLOCAL.Contains(input.Filter) || e.CLASS.Contains(input.Filter) || e.WCOMPNUM1.Contains(input.Filter) || e.LASTNAME.Contains(input.Filter) || e.FIRSTNAME.Contains(input.Filter) || e.STATUS.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.EMPNUMFilter),  e => e.EMPNUM == input.EMPNUMFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NAMEFilter),  e => e.NAME == input.NAMEFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UNIONNUMFilter),  e => e.UNIONNUM == input.UNIONNUMFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UNIONLOCALFilter),  e => e.UNIONLOCAL == input.UNIONLOCALFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CLASSFilter),  e => e.CLASS == input.CLASSFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.WCOMPNUM1Filter),  e => e.WCOMPNUM1 == input.WCOMPNUM1Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LASTNAMEFilter),  e => e.LASTNAME == input.LASTNAMEFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.FIRSTNAMEFilter),  e => e.FIRSTNAME == input.FIRSTNAMEFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.STATUSFilter),  e => e.STATUS == input.STATUSFilter)
						.WhereIf(input.MinPAYRATEFilter != null, e => e.PAYRATE >= input.MinPAYRATEFilter)
						.WhereIf(input.MaxPAYRATEFilter != null, e => e.PAYRATE <= input.MaxPAYRATEFilter);

			var query = (from o in filteredPREMPLOYEES
                         select new GetPREMPLOYEEForViewDto() { 
							PREMPLOYEE = new PREMPLOYEEDto
							{
                                EMPNUM = o.EMPNUM,
                                NAME = o.NAME,
                                UNIONNUM = o.UNIONNUM,
                                UNIONLOCAL = o.UNIONLOCAL,
                                CLASS = o.CLASS,
                                WCOMPNUM1 = o.WCOMPNUM1,
                                LASTNAME = o.LASTNAME,
                                FIRSTNAME = o.FIRSTNAME,
                                STATUS = o.STATUS,
                                PAYRATE = o.PAYRATE,
                                Id = o.Id
							}
						 });


            var premployeeListDtos = await query.ToListAsync();

            return _premployeesExcelExporter.ExportToFile(premployeeListDtos);
         }


    }
}