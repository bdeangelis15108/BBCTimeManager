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
   public class EmployeeReviewListModel : ShiftResourcesDto, INotifyPropertyChanged
    {
        public string resourceName { get; set; }
        public string paytypeCode { get; set; }

        private decimal regAmount { get; set; }
        private decimal oTamount { get; set; }
        private decimal dTamount { get; set; }
        private decimal oT18amount { get; set; }
        private decimal rEG10amount { get; set; }
        private decimal rEG15amount { get; set; }
        private decimal oT10amount { get; set; }
        private decimal aWLamount { get; set; }
        private decimal aWOLamount { get; set; }
        private decimal iLLamount { get; set; }
        private decimal vACamount { get; set; }
        private decimal hOLamount { get; set; }

        public decimal RegAmount
        {
            get { return regAmount; }
            set
            {
                regAmount = value;
                OnPropertyChanged();
            }
        }
        public decimal OTamount
        {
            get { return oTamount; }
            set
            {
                oTamount = value;
                OnPropertyChanged();
            }
        }
        public decimal DTamount
        {
            get { return dTamount; }
            set
            {
                dTamount = value;
                OnPropertyChanged();
            }
        }
        public decimal OT18amount
        {
            get { return oT18amount; }
            set
            {
                oT18amount = value;
                OnPropertyChanged();
            }
        }
        public decimal REG10amount
        {
            get { return rEG10amount; }
            set
            {
                rEG10amount = value;
                OnPropertyChanged();
            }
        }
        public decimal OT10amount
        {
            get { return oT10amount; }
            set
            {
                oT10amount = value;
                OnPropertyChanged();
            }
        }
        public decimal REG15amount
        {
            get { return rEG15amount; }
            set
            {
                rEG15amount = value;
                OnPropertyChanged();
            }
        }
        public decimal AWLamount
        {
            get { return aWLamount; }
            set
            {
                aWLamount = value;
                OnPropertyChanged();
            }
        }
        public decimal AWOLamount
        {
            get { return aWOLamount; }
            set
            {
                aWOLamount = value;
                OnPropertyChanged();
            }
        }
        public decimal ILLamount
        {
            get { return iLLamount; }
            set
            {
                iLLamount = value;
                OnPropertyChanged();
            }
        }
        public decimal VACamount
        {
            get { return vACamount; }
            set
            {
                vACamount = value;
                OnPropertyChanged();
            }
        }
        public decimal HOLamount
        {
            get { return hOLamount; }
            set
            {
                hOLamount = value;
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
