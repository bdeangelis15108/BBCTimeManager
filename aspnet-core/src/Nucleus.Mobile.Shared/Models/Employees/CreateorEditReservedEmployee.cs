using Abp.AutoMapper;
using MvvmHelpers;
using Nucleus.ResourceReservation.Dtos;
using Nucleus.ResourceWorkerInfo.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Nucleus.Models.Employees 
{
    [AutoMapFrom(typeof(CreateOrEditResourceReservationsDto))]
    class CreateorEditReservedEmployee : CreateOrEditResourceReservationsDto, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
