using Abp.AutoMapper;
using MvvmHelpers;
using Nucleus.Resource.Dtos;
using Nucleus.ResourceReservation.Dtos;
using Nucleus.ResourceWorkerInfo.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Nucleus.Models.Employees 
{
    [AutoMapFrom(typeof(ResourceWorkerInfosDto))]
   public class EmployeesListModel : ResourceWorkerInfosDto, INotifyPropertyChanged
    {
        private bool _isChecked;
        private bool isReservedByOther;
        public string WorkerClasees { get; set; }
        public string FullName => FirstName + " " + LastName;
        public string RefPlusClass => RefNumber + "(" + WorkerClasees + ")";

        private string reservedByName { get; set; }
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnPropertyChanged();
            }
        }
        public bool IsReservedByOther
        {
            get { return isReservedByOther; }
            set
            {
                isReservedByOther = value;
                OnPropertyChanged();
            }
        }
        public string ReservedByName
        {
            get
            {
                return reservedByName;
            }
            set
            {
                reservedByName = value;
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
