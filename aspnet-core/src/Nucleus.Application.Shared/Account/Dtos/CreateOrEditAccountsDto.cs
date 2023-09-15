using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.Account.Dtos
{
    public class CreateOrEditAccountsDto : EntityDto<int?>
    {

        [StringLength(AccountsConsts.MaxNameLength, MinimumLength = AccountsConsts.MinNameLength)]
        public string Name { get; set; }

        [StringLength(AccountsConsts.MaxCodeLength, MinimumLength = AccountsConsts.MinCodeLength)]
        public string Code { get; set; }

    }
}