using Abp.Configuration;
using Abp.Extensions;
using Nucleus.ViewModels;
using Nucleus.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Nucleus.Views
{
	public partial class EmployeeReviewView : ContentPage, IXamarinView
    {
		public DateTime selectedDT;
		public EmployeeReviewView()
		{
			InitializeComponent();

		}

        public EmployeeReviewView(DateTime selecteddateTime)
		{
			InitializeComponent();
			var vm = BindingContext as EmployeeReviewViewModel;
			vm.SelectedDate = selecteddateTime;
			vm.SelectedDT= selecteddateTime.ToString("dddd\n MMM dd").ToUpper();
			selectedDT = selecteddateTime;
		}		
		private async void ToolbarItem_Clicked(object sender, EventArgs e)
		{
			Settings.isEmployeeChecked = true;
			var vm = BindingContext as EmployeeReviewViewModel;
			await vm.SaveEmployeeConfirmation();
			await Navigation.PopAsync();
		}
	}
}