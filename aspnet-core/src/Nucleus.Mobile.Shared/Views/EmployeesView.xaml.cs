using Nucleus.Models.Employees;
using Nucleus.Resource.Dtos;
using Nucleus.ResourceWorkerInfo.Dtos;
using Nucleus.ViewModels;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Nucleus.Views
{
	public partial class EmployeesView : ContentPage, IXamarinView
    {
		public EmployeesView ()
		{
			InitializeComponent ();			
			var vm = BindingContext as EmployeesViewModel;
			if(vm.ReservedEmployees.Count() <= 0 )
            {
				bg_reservationEmp.IsVisible = true;
				lvEmployees.IsVisible = false;

			}
            else
            {
				bg_reservationEmp.IsVisible = false;
				lvEmployees.IsVisible = true;
			}

		}
		protected override bool OnBackButtonPressed()
        {
			
			return false;
        }
		private async void ic_addEmp_Clicked(object sender, System.EventArgs e)
		{
			labelBg.IsVisible = true;
			lv_Employees.IsVisible = true;
			listheader.IsVisible = true;
		//	var v = BindingContext as EmployeesViewModel;			
			//await ((EmployeesViewModel)BindingContext).RefreshEmployeesAsync();
		}
		

		private void Button_Clicked(object sender, System.EventArgs e)
		{
			labelBg.IsVisible = false;
			lv_Employees.IsVisible = false;
			listheader.IsVisible = false;
		}

		public async void chkEqup_CheckedChanged(object sender, CheckedChangedEventArgs e)
		{
			var checkbox = (CheckBox)sender;
			var ob = checkbox.BindingContext;
			var v = BindingContext as EmployeesViewModel;
			if (ob != null && checkbox.IsChecked && !v.ReservedEmployees.Any(ex=>ex.ResourcesId == ((EmployeesListModel)ob).ResourcesId))
			{
				v.AddCommand.Execute(ob);
				
			}
			else if (!checkbox.IsChecked)
			{
				var result = await DisplayAlert("Reservation cancellation", "The selected Employee and existing time entries will be removed from timesheets for all dates?", "Confirm", "Cancel");
				if (result.Equals(true))
				{
					v.RemoveCommand.Execute(ob);
				}
				else
				{
					checkbox.IsChecked = true;
				}
			}

			if (v.ReservedEmployees.Count() <= 0)
			{
				bg_reservationEmp.IsVisible = true;
				lvEmployees.IsVisible = false;
			}
			else
			{
				bg_reservationEmp.IsVisible = false;
				lvEmployees.IsVisible = true;
			}
			var vm =BindingContext as EmployeesViewModel;
			lvEmployees.ItemsSource = vm.ReservedEmployees;	
		}
		private async void ImageButton_Clicked(object sender, EventArgs e)
		{
			//var result=await DisplayAlert("Reservation cancellation", "The selected Employee and their existing time entries will be removed from timesheets for all dates?", "Confirm", "Cancel");
           // if (result.Equals(true))
           // {
				var imageButton = (ImageButton)sender;
				var ob = imageButton.BindingContext;
				var vm = BindingContext as EmployeesViewModel;

			if (ob != null)
			{
				vm.RemoveCommand.Execute(ob);

			}
			
			//}

		}

		private async void ToolbarItem_Clicked(object sender, EventArgs e)
		{
			var button = (ToolbarItem)sender;
			var buttonContext = button.BindingContext;
			var vm = BindingContext as EmployeesViewModel;			
			if (vm.ReservedEmployees.Count() > 0)
			{
				//check the reservation is not duplicate
				var isDuplicate= await vm.CheckReservationDuplication(vm.ReservedEmployees);
                if (isDuplicate.Equals(""))
                {
					var result = await DisplayAlert("Confirm Reservation", "Are you sure you want to make this reservation?", "Confirm", "Cancel");
					if (result.Equals(true))
						vm.CreateNewResourceReservedAsync(vm.ReservedEmployees);

                }
                else
                {
					 await DisplayAlert("Error During Resrvation", isDuplicate.ToString() +" Please remove the employee to complete reservation", "Ok","Cancel");
					
				}

			}
		}
		public async void ListView_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
		{
			//if (CrossConnectivity.Current.IsConnected)
			await ((EmployeesViewModel)BindingContext).LoadMoreEmployeeIfNeedsAsync(e.Item as EmployeesListModel);
		}
	}
	



}