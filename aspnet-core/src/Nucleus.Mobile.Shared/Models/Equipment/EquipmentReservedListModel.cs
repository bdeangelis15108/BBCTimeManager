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

namespace Nucleus.Models.Equipment
{
    [AutoMapFrom(typeof(ResourceReservationsDto))]
   public class EquipmentReservedListModel : ResourceReservationsDto, INotifyPropertyChanged
    {
        public ResourcesDto ResourcesDto;
        private bool isReservedByOther { get; set; }
        private string reservedByName { get; set; }
       
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
