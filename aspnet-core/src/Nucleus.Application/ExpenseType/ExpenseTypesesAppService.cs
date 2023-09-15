

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.ExpenseType.Exporting;
using Nucleus.ExpenseType.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.ExpenseType
{
	[AbpAuthorize(AppPermissions.Pages_ExpenseTypeses)]
    public class ExpenseTypesesAppService : NucleusAppServiceBase, IExpenseTypesesAppService
    {
		 private readonly IRepository<ExpenseTypes> _expenseTypesRepository;
		 private readonly IExpenseTypesesExcelExporter _expenseTypesesExcelExporter;
		 

		  public ExpenseTypesesAppService(IRepository<ExpenseTypes> expenseTypesRepository, IExpenseTypesesExcelExporter expenseTypesesExcelExporter ) 
		  {
			_expenseTypesRepository = expenseTypesRepository;
			_expenseTypesesExcelExporter = expenseTypesesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetExpenseTypesForViewDto>> GetAll(GetAllExpenseTypesesInput input)
         {
			
			var filteredExpenseTypeses = _expenseTypesRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(input.MinIconFilter != null, e => e.Icon >= input.MinIconFilter)
						.WhereIf(input.MaxIconFilter != null, e => e.Icon <= input.MaxIconFilter);

			var pagedAndFilteredExpenseTypeses = filteredExpenseTypeses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var expenseTypeses = from o in pagedAndFilteredExpenseTypeses
                         select new GetExpenseTypesForViewDto() {
							ExpenseTypes = new ExpenseTypesDto
							{
                                Name = o.Name,
                                Description = o.Description,
                                Code = o.Code,
                                Icon = o.Icon,
                                Id = o.Id
							}
						};

            var totalCount = await filteredExpenseTypeses.CountAsync();

            return new PagedResultDto<GetExpenseTypesForViewDto>(
                totalCount,
                await expenseTypeses.ToListAsync()
            );
         }
		 
		 public async Task<GetExpenseTypesForViewDto> GetExpenseTypesForView(int id)
         {
            var expenseTypes = await _expenseTypesRepository.GetAsync(id);

            var output = new GetExpenseTypesForViewDto { ExpenseTypes = ObjectMapper.Map<ExpenseTypesDto>(expenseTypes) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ExpenseTypeses_Edit)]
		 public async Task<GetExpenseTypesForEditOutput> GetExpenseTypesForEdit(EntityDto input)
         {
            var expenseTypes = await _expenseTypesRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetExpenseTypesForEditOutput {ExpenseTypes = ObjectMapper.Map<CreateOrEditExpenseTypesDto>(expenseTypes)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditExpenseTypesDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ExpenseTypeses_Create)]
		 protected virtual async Task Create(CreateOrEditExpenseTypesDto input)
         {
            var expenseTypes = ObjectMapper.Map<ExpenseTypes>(input);

			

            await _expenseTypesRepository.InsertAsync(expenseTypes);
         }

		 [AbpAuthorize(AppPermissions.Pages_ExpenseTypeses_Edit)]
		 protected virtual async Task Update(CreateOrEditExpenseTypesDto input)
         {
            var expenseTypes = await _expenseTypesRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, expenseTypes);
         }

		 [AbpAuthorize(AppPermissions.Pages_ExpenseTypeses_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _expenseTypesRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetExpenseTypesesToExcel(GetAllExpenseTypesesForExcelInput input)
         {
			
			var filteredExpenseTypeses = _expenseTypesRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Code.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(input.MinIconFilter != null, e => e.Icon >= input.MinIconFilter)
						.WhereIf(input.MaxIconFilter != null, e => e.Icon <= input.MaxIconFilter);

			var query = (from o in filteredExpenseTypeses
                         select new GetExpenseTypesForViewDto() { 
							ExpenseTypes = new ExpenseTypesDto
							{
                                Name = o.Name,
                                Description = o.Description,
                                Code = o.Code,
                                Icon = o.Icon,
                                Id = o.Id
							}
						 });


            var expenseTypesListDtos = await query.ToListAsync();

            return _expenseTypesesExcelExporter.ExportToFile(expenseTypesListDtos);
         }


    }
}