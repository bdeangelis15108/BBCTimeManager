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

namespace Nucleus.Models.TimesheetPerShift
{
    
   public class TimesheetPerShiftListViewModel : INotifyPropertyChanged
    {
        private bool _isChecked;
        private string name;
        private int id;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        public int Id
        {
            get { return id; }
            set { id = value;OnPropertyChanged(); }
        }
        private void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
