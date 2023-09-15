using Nucleus.ShiftResource;
using Nucleus.ExpenseType;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.ShiftExpense.Exporting;
using Nucleus.ShiftExpense.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.ShiftExpense
{
	[AbpAuthorize(AppPermissions.Pages_ShiftExpenses)]
    public class ShiftExpensesAppService : NucleusAppServiceBase, IShiftExpensesAppService
    {
		 private readonly IRepository<ShiftExpenses> _shiftExpensesRepository;
		 private readonly IShiftExpensesExcelExporter _shiftExpensesExcelExporter;
		 private readonly IRepository<ShiftResources,int> _lookup_shiftResourcesRepository;
		 private readonly IRepository<ExpenseTypes,int> _lookup_expenseTypesRepository;
		 

		  public ShiftExpensesAppService(IRepository<ShiftExpenses> shiftExpensesRepository, IShiftExpensesExcelExporter shiftExpensesExcelExporter , IRepository<ShiftResources, int> lookup_shiftResourcesRepository, IRepository<ExpenseTypes, int> lookup_expenseTypesRepository) 
		  {
			_shiftExpensesRepository = shiftExpensesRepository;
			_shiftExpensesExcelExporter = shiftExpensesExcelExporter;
			_lookup_shiftResourcesRepository = lookup_shiftResourcesRepository;
		_lookup_expenseTypesRepository = lookup_expenseTypesRepository;
		
		  }

		 public async Task<PagedResultDto<GetShiftExpensesForViewDto>> GetAll(GetAllShiftExpensesInput input)
         {
			
			var filteredShiftExpenses = _shiftExpensesRepository.GetAll()
						.Include( e => e.ShiftResourcesFk)
						.Include( e => e.ExpenseTypesFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
						.WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ShiftResourcesNameFilter), e => e.ShiftResourcesFk != null && e.ShiftResourcesFk.Name == input.ShiftResourcesNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ExpenseTypesNameFilter), e => e.ExpenseTypesFk != null && e.ExpenseTypesFk.Name == input.ExpenseTypesNameFilter)
                        .WhereIf(!input.ShiftResourcesIdFilter.Equals(0), e => e.ShiftResourcesId != null && e.ShiftResourcesId .Equals(input.ShiftResourcesIdFilter));

			var pagedAndFilteredShiftExpenses = filteredShiftExpenses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var shiftExpenses = from o in pagedAndFilteredShiftExpenses
                         join o1 in _lookup_shiftResourcesRepository.GetAll() on o.ShiftResourcesId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_expenseTypesRepository.GetAll() on o.ExpenseTypesId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetShiftExpensesForViewDto() {
							ShiftExpenses = new ShiftExpensesDto
							{
                                Name = o.Name,
                                Amount = o.Amount,
                                Id = o.Id,
                                ExpenseTypesId = o.ExpenseTypesId,
                                ShiftResourcesId = o.ShiftResourcesId
							},
                         	ShiftResourcesName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                         	ExpenseTypesName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredShiftExpenses.CountAsync();

            return new PagedResultDto<GetShiftExpensesForViewDto>(
                totalCount,
                await shiftExpenses.ToListAsync()
            );
         }
		 
		 public async Task<GetShiftExpensesForViewDto> GetShiftExpensesForView(int id)
         {
            var shiftExpenses = await _shiftExpensesRepository.GetAsync(id);

            var output = new GetShiftExpensesForViewDto { ShiftExpenses = ObjectMapper.Map<ShiftExpensesDto>(shiftExpenses) };

		    if (output.ShiftExpenses.ShiftResourcesId != null)
            {
                var _lookupShiftResources = await _lookup_shiftResourcesRepository.FirstOrDefaultAsync((int)output.ShiftExpenses.ShiftResourcesId);
                output.ShiftResourcesName = _lookupShiftResources?.Name?.ToString();
            }

		    if (output.ShiftExpenses.ExpenseTypesId != null)
            {
                var _lookupExpenseTypes = await _lookup_expenseTypesRepository.FirstOrDefaultAsync((int)output.ShiftExpenses.ExpenseTypesId);
                output.ExpenseTypesName = _lookupExpenseTypes?.Name?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ShiftExpenses_Edit)]
		 public async Task<GetShiftExpensesForEditOutput> GetShiftExpensesForEdit(EntityDto input)
         {
            var shiftExpenses = await _shiftExpensesRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetShiftExpensesForEditOutput {ShiftExpenses = ObjectMapper.Map<CreateOrEditShiftExpensesDto>(shiftExpenses)};

		    if (output.ShiftExpenses.ShiftResourcesId != null)
            {
                var _lookupShiftResources = await _lookup_shiftResourcesRepository.FirstOrDefaultAsync((int)output.ShiftExpenses.ShiftResourcesId);
                output.ShiftResourcesName = _lookupShiftResources?.Name?.ToString();
            }

		    if (output.ShiftExpenses.ExpenseTypesId != null)
            {
                var _lookupExpenseTypes = await _lookup_expenseTypesRepository.FirstOrDefaultAsync((int)output.ShiftExpenses.ExpenseTypesId);
                output.ExpenseTypesName = _lookupExpenseTypes?.Name?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditShiftExpensesDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ShiftExpenses_Create)]
		 protected virtual async Task Create(CreateOrEditShiftExpensesDto input)
         {
            var shiftExpenses = ObjectMapper.Map<ShiftExpenses>(input);

			

            await _shiftExpensesRepository.InsertAsync(shiftExpenses);
         }

		 [AbpAuthorize(AppPermissions.Pages_ShiftExpenses_Edit)]
		 protected virtual async Task Update(CreateOrEditShiftExpensesDto input)
         {
            var shiftExpenses = await _shiftExpensesRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, shiftExpenses);
         }

		 [AbpAuthorize(AppPermissions.Pages_ShiftExpenses_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _shiftExpensesRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetShiftExpensesToExcel(GetAllShiftExpensesForExcelInput input)
         {
			
			var filteredShiftExpenses = _shiftExpensesRepository.GetAll()
						.Include( e => e.ShiftResourcesFk)
						.Include( e => e.ExpenseTypesFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
						.WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ShiftResourcesNameFilter), e => e.ShiftResourcesFk != null && e.ShiftResourcesFk.Name == input.ShiftResourcesNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ExpenseTypesNameFilter), e => e.ExpenseTypesFk != null && e.ExpenseTypesFk.Name == input.ExpenseTypesNameFilter);

			var query = (from o in filteredShiftExpenses
                         join o1 in _lookup_shiftResourcesRepository.GetAll() on o.ShiftResourcesId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_expenseTypesRepository.GetAll() on o.ExpenseTypesId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetShiftExpensesForViewDto() { 
							ShiftExpenses = new ShiftExpensesDto
							{
                                Name = o.Name,
                                Amount = o.Amount,
                                Id = o.Id
							},
                         	ShiftResourcesName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                         	ExpenseTypesName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
						 });


            var shiftExpensesListDtos = await query.ToListAsync();

            return _shiftExpensesExcelExporter.ExportToFile(shiftExpensesListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_ShiftExpenses)]
         public async Task<PagedResultDto<ShiftExpensesShiftResourcesLookupTableDto>> GetAllShiftResourcesForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_shiftResourcesRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var shiftResourcesList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ShiftExpensesShiftResourcesLookupTableDto>();
			foreach(var shiftResources in shiftResourcesList){
				lookupTableDtoList.Add(new ShiftExpensesShiftResourcesLookupTableDto
				{
					Id = shiftResources.Id,
					DisplayName = shiftResources.Name?.ToString()
				});
			}

            return new PagedResultDto<ShiftExpensesShiftResourcesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_ShiftExpenses)]
         public async Task<PagedResultDto<ShiftExpensesExpenseTypesLookupTableDto>> GetAllExpenseTypesForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_expenseTypesRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var expenseTypesList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ShiftExpensesExpenseTypesLookupTableDto>();
			foreach(var expenseTypes in expenseTypesList){
				lookupTableDtoList.Add(new ShiftExpensesExpenseTypesLookupTableDto
				{
					Id = expenseTypes.Id,
					DisplayName = expenseTypes.Name?.ToString()
				});
			}

            return new PagedResultDto<ShiftExpensesExpenseTypesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}