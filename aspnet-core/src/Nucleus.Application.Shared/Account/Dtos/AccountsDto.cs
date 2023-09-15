using System;
using Abp.Application.Services.Dto;

namespace Nucleus.Account.Dtos
{
    public class AccountsDto : EntityDto
    {
        public string Name { get; set; }

        public string Code { get; set; }

    }
}