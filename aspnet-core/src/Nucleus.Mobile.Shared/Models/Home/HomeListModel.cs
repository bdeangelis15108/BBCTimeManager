using Abp.AutoMapper;
using MvvmHelpers;
using Nucleus.ResourceReservation.Dtos;
using Nucleus.ResourceWorkerInfo.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Nucleus.Models.Home 
{
    public class HomeListModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string DateofMonthandYear { get; set; }
        public string Date { get; set; }
        public string Day { get; set; }
        public DateTime actualDate {get;set;}
        private bool isSubmitted { get; set; }
        public bool IsSubmitted
        {
            get
            {
                return isSubmitted;
            }
            set
            {
                isSubmitted = value;
                OnPropertyChanged();
            }
        }
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
