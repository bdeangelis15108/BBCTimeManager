using Abp.Application.Services.Dto;
using System;

namespace Nucleus.Account.Dtos
{
    public class GetAllAccountsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string CodeFilter { get; set; }

    }
}