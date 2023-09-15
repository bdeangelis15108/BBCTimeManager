using Abp.AutoMapper;
using MvvmHelpers;
using Nucleus.Resource.Dtos;
using Nucleus.ResourceReservation.Dtos;
using Nucleus.ResourceWorkerInfo.Dtos;
using Nucleus.ShiftResource.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Nucleus.Models.Employees 
{
    [AutoMapFrom(typeof(ShiftResourcesDto))]
   public class EquipmentReviewListModel : ShiftResourcesDto, INotifyPropertyChanged
    {
        public string resourceName { get; set; }
        public string resourceCode { get; set; }
        private string job { get; set; }
        private int rowNumb { get; set; }
        private int columnNumb { get; set; }
        private decimal totalUsageAmount { get; set; }

        public string Job
        {
            get
            {
                return job;
            }
            set
            {
                job = value;
                OnPropertyChanged();
            }
        }
        public int RowNumb
        {
            get
            {
                return rowNumb;
            }
            set
            {
                rowNumb = value;
                OnPropertyChanged();
            }
        }
        public int ColumnNumb
        {
            get
            {
                return columnNumb;
            }
            set
            {
                columnNumb = value;
                OnPropertyChanged();
            }
        }
        public decimal TotalUsageAmount
        {
            get
            {
                return totalUsageAmount;
            }
            set
            {
                totalUsageAmount = value;
                OnPropertyChanged();
            }
        }
        private void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
