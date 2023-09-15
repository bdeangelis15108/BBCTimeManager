using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.Account
{
    [Table("Accounts")]
    [Audited]
    public class Accounts : Entity
    {

        [StringLength(AccountsConsts.MaxNameLength, MinimumLength = AccountsConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [StringLength(AccountsConsts.MaxCodeLength, MinimumLength = AccountsConsts.MinCodeLength)]
        public virtual string Code { get; set; }

    }
}