using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.Account.Exporting;
using Nucleus.Account.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.Account
{
    [AbpAuthorize(AppPermissions.Pages_Accounts)]
    public class AccountsAppService : NucleusAppServiceBase, IAccountsAppService
    {
        private readonly IRepository<Accounts> _accountsRepository;
        private readonly IAccountsExcelExporter _accountsExcelExporter;

        public AccountsAppService(IRepository<Accounts> accountsRepository, IAccountsExcelExporter accountsExcelExporter)
        {
            _accountsRepository = accountsRepository;
            _accountsExcelExporter = accountsExcelExporter;

        }

        public async Task<PagedResultDto<GetAccountsForViewDto>> GetAll(GetAllAccountsInput input)
        {

            var filteredAccounts = _accountsRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter);

            var pagedAndFilteredAccounts = filteredAccounts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var accounts = from o in pagedAndFilteredAccounts
                           select new GetAccountsForViewDto()
                           {
                               Accounts = new AccountsDto
                               {
                                   Name = o.Name,
                                   Code = o.Code,
                                   Id = o.Id
                               }
                           };

            var totalCount = await filteredAccounts.CountAsync();

            return new PagedResultDto<GetAccountsForViewDto>(
                totalCount,
                await accounts.ToListAsync()
            );
        }

        public async Task<GetAccountsForViewDto> GetAccountsForView(int id)
        {
            var accounts = await _accountsRepository.GetAsync(id);

            var output = new GetAccountsForViewDto { Accounts = ObjectMapper.Map<AccountsDto>(accounts) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Accounts_Edit)]
        public async Task<GetAccountsForEditOutput> GetAccountsForEdit(EntityDto input)
        {
            var accounts = await _accountsRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetAccountsForEditOutput { Accounts = ObjectMapper.Map<CreateOrEditAccountsDto>(accounts) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditAccountsDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Accounts_Create)]
        protected virtual async Task Create(CreateOrEditAccountsDto input)
        {
            var accounts = ObjectMapper.Map<Accounts>(input);

            await _accountsRepository.InsertAsync(accounts);
        }

        [AbpAuthorize(AppPermissions.Pages_Accounts_Edit)]
        protected virtual async Task Update(CreateOrEditAccountsDto input)
        {
            var accounts = await _accountsRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, accounts);
        }

        [AbpAuthorize(AppPermissions.Pages_Accounts_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _accountsRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetAccountsToExcel(GetAllAccountsForExcelInput input)
        {

            var filteredAccounts = _accountsRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter);

            var query = (from o in filteredAccounts
                         select new GetAccountsForViewDto()
                         {
                             Accounts = new AccountsDto
                             {
                                 Name = o.Name,
                                 Code = o.Code,
                                 Id = o.Id
                             }
                         });

            var accountsListDtos = await query.ToListAsync();

            return _accountsExcelExporter.ExportToFile(accountsListDtos);
        }

    }
}