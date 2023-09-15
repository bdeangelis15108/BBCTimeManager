using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.Account.Dtos
{
    public class GetAccountsForEditOutput
    {
        public CreateOrEditAccountsDto Accounts { get; set; }

    }
}