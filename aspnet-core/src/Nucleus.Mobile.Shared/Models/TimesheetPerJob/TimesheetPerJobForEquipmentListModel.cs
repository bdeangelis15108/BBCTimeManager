using Abp.AutoMapper;
using MvvmHelpers;
using Nucleus.ExpenseType.Dtos;
using Nucleus.PayType.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Nucleus.Models.TimesheetPerJob
{
    public class TimesheetPerJobForEquipmentListModel :  INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string ResourceName { get; set; }
        public string JobName { get; set; }
        public string SelectedDate { get; set; }
        public string Type { get; set; }
        public PayTypesDto SelectedPickerItem { get; set; }

        // TODO: Any other properties to be bound to the grid?
    }
}
