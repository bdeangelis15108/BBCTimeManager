using Abp.AutoMapper;
using Abp.Extensions;
using MvvmHelpers;
using Nucleus.ExpenseType.Dtos;
using Nucleus.JobCategory.Dtos;
using Nucleus.JobPhaseCode.Dtos;
using Nucleus.PayType.Dtos;
using Nucleus.Resource.Dtos;
using Nucleus.WorkerClasee.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace Nucleus.Models.TimesheetPerJob
{
    public class TimesheetPerJobListModel :  INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool isValid { get; set; }
        private bool isPaytype2Valid { get; set; }
        private bool isPaytype3Valid { get; set; }
        private bool isPerDimValid { get; set; }
        private bool isMiscValid { get; set; }
        public int shiftResourceId { get; set; }
        private int timesheetId { get; set; }
        private int shiftId { get; set; }
        public string ResourceName { get; set; }
        public int? ResourceId { get; set; }
        public string JobName { get; set; }
        private WorkerClaseesDto workerClass { get; set; }
        public string Type { get; set; }
        private PayTypesDto section1 { get; set; }
        private PayTypesDto section2 { get; set; }
        private PayTypesDto section3 { get; set; }
        private JobPhaseCodesDto selectedPhaseCode { get; set; }
        private JobCategoriesDto selectedJobCategoryCode { get; set; }
        private string selectedDesctiption { get; set; }
        public string SelectedDate { get; set; }
        public ExpenseTypesDto SelectedExpenseItem { get; set; }
        private string paytype1Text { get; set; }
        private string paytype2Text { get; set; }
        private string paytype3Text { get; set; }
        private string perDimText { get; set; }
        private string miscText { get; set; }
        private string equipmentUsage { get; set; }
        public string EquipmentUsage { get => equipmentUsage; set { equipmentUsage = value; OnPropertyChanged(); } }
        public int trdColWidth { get; set; }
        public string paytypeRegEx = @"^(?!\s*$)[0-9]{1}(\.[0-9]{0,1}[0-9]{0,1})?$|^[1]{1}[0-9]{1}(\.[0-9]{0,1}[0-9]{0,1})?$|^[2]{1}[0-4]{1}(\.[0]{0,1}[0]{0,1})?$";
        // TODO: Any other properties to be bound to the grid?

        private Color _UsageColor { get; set; }
        public Color UsageColor
        {
            get => _UsageColor;
            set
            {
                _UsageColor = value;
                OnPropertyChanged();
            }
        }

        private Color _paytype1Color { get; set; }
        public Color paytype1Color
        {
            get => _paytype1Color;
            set
            {
                _paytype1Color = value;
                OnPropertyChanged();
            }
        }

        private Color _paytype2Color { get; set; }
        public Color paytype2Color
        {
            get => _paytype2Color;
            set
            {
                _paytype2Color = value;
                OnPropertyChanged();
            }
        }

        private Color _paytype3Color { get; set; }
        public Color paytype3Color
        {
            get => _paytype3Color;
            set
            {
                _paytype3Color = value;
                OnPropertyChanged();
            }
        }

        private Color _perDimColor { get; set; }
        public Color perDimColor
        {
            get => _perDimColor;
            set
            {
                _perDimColor = value;
                OnPropertyChanged();
            }
        }

        private Color _miscColor { get; set; }
        public Color miscColor
        {
            get => _miscColor;
            set
            {
                _miscColor = value;
                OnPropertyChanged();
            }
        }

        private bool _UsageEnabled { get; set; }
        public bool UsageEnabled
        {
            get => _UsageEnabled;
            set
            {
                _UsageEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _paytype1Enabled { get; set; }
        public bool paytype1Enabled
        {
            get => _paytype1Enabled;
            set
            {
                _paytype1Enabled = value;
                OnPropertyChanged();
            }
        }

        private bool _paytype2Enabled { get; set; }
        public bool paytype2Enabled
        {
            get => _paytype2Enabled;
            set
            {
                _paytype2Enabled = value;
                OnPropertyChanged();
            }
        }

        private bool _paytype3Enabled { get; set; }
        public bool paytype3Enabled
        {
            get => _paytype3Enabled;
            set
            {
                _paytype3Enabled = value;
                OnPropertyChanged();
            }
        }

        private bool _perdimEnabled { get; set; }
        public bool perdimEnabled
        {
            get => _perdimEnabled;
            set
            {
                _perdimEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _miscEnabled { get; set; }
        public bool miscEnabled
        {
            get => _miscEnabled;
            set
            {
                _miscEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsValid
        {
            get
            {
                return isValid;
            }
            set
            {
                isValid = value;
                OnPropertyChanged();
            }
        }
        public bool IsPaytype2Valid
        {
            get
            {
                return isPaytype2Valid;
            }
            set
            {
                isPaytype2Valid = value;
                OnPropertyChanged();
            }
        }
        public bool IsPaytype3Valid
        {
            get
            {
                return isPaytype3Valid;
            }
            set
            {
                isPaytype3Valid = value;
                OnPropertyChanged();
            }
        }
        public bool IsPerDimValid
        {
            get
            {
                return isPerDimValid;
            }
            set
            {
                isPerDimValid = value;
                OnPropertyChanged();
            }
        }
        public bool IsMiscValid
        {
            get
            {
                return isMiscValid;
            }
            set
            {
                isMiscValid = value;
                OnPropertyChanged();
            }
        }
        public PayTypesDto Section1
        {
            get { return section1; }
            set
            {
                section1 = value;
                OnPropertyChanged();
            }
        }
        public PayTypesDto Section2
        {
            get { return section2; }
            set
            {
                section2 = value;
                OnPropertyChanged();
            }
        }
        public PayTypesDto Section3
        {
            get { return section3; }
            set
            {
                section3 = value;
                OnPropertyChanged();
            }
        }
        public JobPhaseCodesDto SelectedPhaseCode
        {
            get { return selectedPhaseCode; }
            set
            {
                selectedPhaseCode = value;
                OnPropertyChanged();
            }
        }
        [System.ComponentModel.DataAnnotations.Required]
        public string Paytype1Text
        {
            
        get { return paytype1Text; }
            set
            {
                paytype1Text = value;
                OnPropertyChanged();
                var regex = new Regex(paytypeRegEx);
                IsValid = value.IsNullOrEmpty() ? true : false;
                if (regex.IsMatch(value.ToString()))
                {
                    IsValid = false;
                }
                else
                {   
                    IsValid = true;

                }
            }
        }
        [System.ComponentModel.DataAnnotations.Required]
        public string Paytype2Text
        {
            get { return paytype2Text; }
            set
            {
                paytype2Text = value;
                OnPropertyChanged();
                var regex = new Regex(paytypeRegEx);
                IsPaytype2Valid = value.IsNullOrEmpty() ? true : false;
                if (regex.IsMatch(value.ToString()))
                {
                    IsPaytype2Valid = false;
                }
                else
                {
                    IsPaytype2Valid = true;

                }
            }
        }
        public string Paytype3Text
        {
            get { return paytype3Text; }
            set
            {
                paytype3Text = value;
                OnPropertyChanged();
                var regex = new Regex(paytypeRegEx);
                IsPaytype3Valid = value.IsNullOrEmpty() ? true : false;
                if (regex.IsMatch(value.ToString()))
                {
                    IsPaytype3Valid = false;
                }
                else
                {
                    IsPaytype3Valid = true;

                }
            }
        }
        public string MiscText
        {
            get { return miscText; }
            set
            {
                miscText = value;
                OnPropertyChanged();
                var regex = new Regex(@"^[0-9]{1,3}(\.[0-9]{0,1}[0-9]{0,1})?$|^[1-9]{1}[0-9]{1}[0-9]{1}[0-9]{1}(\.[0-9]{0,1}[0-9]{0,1})?$");
                IsMiscValid = value.IsNullOrEmpty() ? true : false;
                if (regex.IsMatch(value.ToString()))
                {
                    IsMiscValid = false;
                }
                else
                {
                    IsMiscValid = true;

                }
            }
        }
        public string PerDimText
        {
            get { return perDimText; }
            set
            {
                perDimText = value;
                OnPropertyChanged();
                var regex = new Regex(@"^[0-9]{1,2}(\.[0-9]{0,1}[0-9]{0,1})?$|^[1-4]{1}[0-9]{1}[0-9]{1}(\.[0-9]{0,1}[0-9]{0,1})?$|^500(\.[0]{0,1}[0]{0,1})?$");
                IsPerDimValid= value.IsNullOrEmpty() ? true : false;
                if (regex.IsMatch(value.ToString()))
                {
                    IsPerDimValid = false;
                }
                else
                {
                    IsPerDimValid = true;

                }
            }
        }
        public string SelectedDesctiption
        {
            get { return selectedDesctiption; }
            set
            {
                selectedDesctiption = value;
                OnPropertyChanged();
            }
        }
        public JobCategoriesDto SelectedJobCategoryCode
        {
            get { return selectedJobCategoryCode; }
            set
            {
                selectedJobCategoryCode = value;
                OnPropertyChanged();
            }
        }

        public int TimesheetId {
            get { return timesheetId; }
            set { timesheetId = value;OnPropertyChanged(); }
        }
        public WorkerClaseesDto WorkerClass
        {
            get { return workerClass; }
            set { workerClass = value; OnPropertyChanged(); }
        }
        public int ShiftId {
            get { return shiftId; } set { shiftId = value;OnPropertyChanged(); } 
        }
        private void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
