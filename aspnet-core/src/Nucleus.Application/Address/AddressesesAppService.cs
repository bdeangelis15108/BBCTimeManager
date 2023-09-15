

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.Address.Exporting;
using Nucleus.Address.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.Address
{
	[AbpAuthorize(AppPermissions.Pages_Addresseses)]
    public class AddressesesAppService : NucleusAppServiceBase, IAddressesesAppService
    {
		 private readonly IRepository<Addresses> _addressesRepository;
		 private readonly IAddressesesExcelExporter _addressesesExcelExporter;
		 

		  public AddressesesAppService(IRepository<Addresses> addressesRepository, IAddressesesExcelExporter addressesesExcelExporter ) 
		  {
			_addressesRepository = addressesRepository;
			_addressesesExcelExporter = addressesesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetAddressesForViewDto>> GetAll(GetAllAddressesesInput input)
         {
			
			var filteredAddresseses = _addressesRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Linne1.Contains(input.Filter) || e.Line2.Contains(input.Filter) || e.City.Contains(input.Filter) || e.State.Contains(input.Filter) || e.Zip.Contains(input.Filter) || e.Lan.Contains(input.Filter) || e.Lat.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.Linne1Filter),  e => e.Linne1 == input.Linne1Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Line2Filter),  e => e.Line2 == input.Line2Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter),  e => e.City == input.CityFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.StateFilter),  e => e.State == input.StateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ZipFilter),  e => e.Zip == input.ZipFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LanFilter),  e => e.Lan == input.LanFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LatFilter),  e => e.Lat == input.LatFilter);

			var pagedAndFilteredAddresseses = filteredAddresseses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var addresseses = from o in pagedAndFilteredAddresseses
                         select new GetAddressesForViewDto() {
							Addresses = new AddressesDto
							{
                                Linne1 = o.Linne1,
                                Line2 = o.Line2,
                                City = o.City,
                                State = o.State,
                                Zip = o.Zip,
                                Lan = o.Lan,
                                Lat = o.Lat,
                                Id = o.Id
							}
						};

            var totalCount = await filteredAddresseses.CountAsync();

            return new PagedResultDto<GetAddressesForViewDto>(
                totalCount,
                await addresseses.ToListAsync()
            );
         }
		 
		 public async Task<GetAddressesForViewDto> GetAddressesForView(int id)
         {
            var addresses = await _addressesRepository.GetAsync(id);

            var output = new GetAddressesForViewDto { Addresses = ObjectMapper.Map<AddressesDto>(addresses) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Addresseses_Edit)]
		 public async Task<GetAddressesForEditOutput> GetAddressesForEdit(EntityDto input)
         {
            var addresses = await _addressesRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetAddressesForEditOutput {Addresses = ObjectMapper.Map<CreateOrEditAddressesDto>(addresses)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditAddressesDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Addresseses_Create)]
		 protected virtual async Task Create(CreateOrEditAddressesDto input)
         {
            var addresses = ObjectMapper.Map<Addresses>(input);

			

            await _addressesRepository.InsertAsync(addresses);
         }

		 [AbpAuthorize(AppPermissions.Pages_Addresseses_Edit)]
		 protected virtual async Task Update(CreateOrEditAddressesDto input)
         {
            var addresses = await _addressesRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, addresses);
         }

		 [AbpAuthorize(AppPermissions.Pages_Addresseses_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _addressesRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetAddressesesToExcel(GetAllAddressesesForExcelInput input)
         {
			
			var filteredAddresseses = _addressesRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Linne1.Contains(input.Filter) || e.Line2.Contains(input.Filter) || e.City.Contains(input.Filter) || e.State.Contains(input.Filter) || e.Zip.Contains(input.Filter) || e.Lan.Contains(input.Filter) || e.Lat.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.Linne1Filter),  e => e.Linne1 == input.Linne1Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Line2Filter),  e => e.Line2 == input.Line2Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter),  e => e.City == input.CityFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.StateFilter),  e => e.State == input.StateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ZipFilter),  e => e.Zip == input.ZipFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LanFilter),  e => e.Lan == input.LanFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LatFilter),  e => e.Lat == input.LatFilter);

			var query = (from o in filteredAddresseses
                         select new GetAddressesForViewDto() { 
							Addresses = new AddressesDto
							{
                                Linne1 = o.Linne1,
                                Line2 = o.Line2,
                                City = o.City,
                                State = o.State,
                                Zip = o.Zip,
                                Lan = o.Lan,
                                Lat = o.Lat,
                                Id = o.Id
							}
						 });


            var addressesListDtos = await query.ToListAsync();

            return _addressesesExcelExporter.ExportToFile(addressesListDtos);
         }


    }
}