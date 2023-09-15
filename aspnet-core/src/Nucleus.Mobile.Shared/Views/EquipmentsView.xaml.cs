using Nucleus.Models.Equipment;
using Nucleus.Resource.Dtos;
using Nucleus.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Nucleus.Views
{
	public partial class EquipmentsView : ContentPage, IXamarinView
    {
	
		public EquipmentsView()
		{
			InitializeComponent ();
			var vm = BindingContext as EquipmentsViewModel;
			if (vm.ReservedEquipment.Count() <= 0)
			{
				bg_reservationEqp.IsVisible = true;
				lvEquipment.IsVisible = false;
			}
			else
			{
				bg_reservationEqp.IsVisible = false;
				lvEquipment.IsVisible = true;
			}

		}

		public void ic_addEmp_Clicked(object sender, System.EventArgs e)
		{
			labelBg.IsVisible = true;
			lv_Equipment.IsVisible = true;
			listheader.IsVisible = true;
		}

		private void Button_Clicked(object sender, System.EventArgs e)
		{
			labelBg.IsVisible = false;
			lv_Equipment.IsVisible = false;
			listheader.IsVisible = false;
		}

		public async void chkEqup_CheckedChanged(object sender, CheckedChangedEventArgs e)
		{

			var checkbox = (CheckBox)sender;
			var ob = checkbox.BindingContext;
			var v = BindingContext as EquipmentsViewModel;
			if (ob != null && checkbox.IsChecked && !v.ReservedEquipment.Any(ex => ex.Id == ((EquipmentListModel)ob).Id))
				v.AddCommand.Execute(ob);
			else if (!checkbox.IsChecked)
            {
				var result = await DisplayAlert("Reservation cancellation", "The selected Equipment and existing time entries will be removed from timesheets for all dates?", "Confirm", "Cancel");
				if (result.Equals(true))
				{
					v.RemoveCommand.Execute(ob);
                }
                else
                {
					checkbox.IsChecked = true;
                }
			}
				
			if (v.ReservedEquipment.Count() <= 0)
			{
				bg_reservationEqp.IsVisible = true;
				lvEquipment.IsVisible = false;
			}
            else
            {
				bg_reservationEqp.IsVisible = false;
				lvEquipment.IsVisible = true;
			}
			var vm = BindingContext as EquipmentsViewModel;
			lvEquipment.ItemsSource = vm.ReservedEquipment;
		}
		private async void ImageButton_Clicked_1(object sender, System.EventArgs e)
		{
			//var result = await DisplayAlert("Reservation cancellation", "The selected Equipment and existing time entries will be removed from timesheets for all dates?", "Confirm", "Cancel");
            //if (result.Equals(true))
           // {
				var imageButton = (ImageButton)sender;
				var ob = imageButton.BindingContext;
				var vm = BindingContext as EquipmentsViewModel;
				if (ob != null)
				{
					vm.RemoveCommand.Execute(ob);
				}
			//}

		}

		private async void ToolbarItem_Clicked(object sender, System.EventArgs e)
		{
			var button = (ToolbarItem)sender;
			var buttonContext = button.BindingContext;
			var vm = BindingContext as EquipmentsViewModel;
			if (vm.ReservedEquipment.Count() > 0)
			{
				
				var result=await DisplayAlert("Confirm Reservation", "Are you sure you want to make this reservation?", "Confirm","Cancel");
				if(result.Equals(true))
					vm.CreateNewResourceReservedAsync(vm.ReservedEquipment);
				await Navigation.PopAsync();
			}
		}
		public async void ListView_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
		{
			await ((EquipmentsViewModel)BindingContext).LoadMoreEquipmentIfNeedsAsync(e.Item as ResourcesDto);
		}
	}
}