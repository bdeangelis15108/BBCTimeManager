using Abp.AutoMapper;
using MvvmHelpers;
using Nucleus.ExpenseType.Dtos;
using Nucleus.PayType.Dtos;
using Nucleus.Resource.Dtos;
using Nucleus.ResourceReservation.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Permissions;
using System.Text;

namespace Nucleus.Models.Employees
{
    [AutoMapFrom(typeof(ResourceReservationsDto))]
    public class EmployeesresevedResorses : ResourceReservationsDto, INotifyPropertyChanged
    {
        private string reservedBy { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public ResourcesDto resourcesDto { get; set; }
        public string ReservedBy
        {
            get {
                return reservedBy;
            }
            set
            {
                reservedBy = value;
                
            }
         }
       
       

        // TODO: Any other properties to be bound to the grid?
    }
}
