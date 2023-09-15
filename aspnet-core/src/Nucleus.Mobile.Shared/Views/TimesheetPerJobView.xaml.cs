using Abp.Extensions;
using Nucleus.ExpenseType.Dtos;
using Nucleus.Job.Dtos;
using Nucleus.Models.TimesheetPerJob;
using Nucleus.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Nucleus.Views {

	public partial class TimesheetPerJobView : ContentPage, IXamarinView
	{
		public Button btn_description;
		public TimesheetPerJobView()
		{
			InitializeComponent();
		}

		public TimesheetPerJobView(JobsDto jobDto, DateTime dateTime)
		{
			InitializeComponent();
			var vm = BindingContext as TimesheetPerJobViewModel;
			vm.selectedJobandDate.ScheduledStart = dateTime;
			vm.selectedJobandDate.JobsId = jobDto.Id;
			vm.selectedJobandDate.Name = jobDto.Name;
			vm.SelectedJob.Execute(jobDto);
			//btn_description.Text= vm.selectedDescription;
			if(vm.TimesheetLineItems.Count() <= 0)
            {
				bg_timesheet.IsVisible = true;

            }
            else
            {
				bg_timesheet.IsVisible = false;
            }

			
		}

		private void Arrow_OnTapped(object sender, EventArgs e)
		{
			var picker = (Picker)((Image)sender).Parent.FindByName("pk_PayType1");
			picker.Focus();
		}
		private void Arrow_OnTapped_pk2(object sender, EventArgs e)
		{
			var picker = (Picker)((Image)sender).Parent.FindByName("pk_PayType2");
			picker.Focus();
		}
		private void Arrow_OnTapped_pk3(object sender, EventArgs e)
		{
			var picker = (Picker)((Image)sender).Parent.FindByName("pk_PayType3");
			picker.Focus();
		}
		private void Arrow_OnTapped_PhaseCode(object sender, EventArgs e)
		{
			var picker = (Picker)((Image)sender).Parent.FindByName("pk_Phasecode");
			picker.Focus();
		}
		private void Arrow_OnTapped_PhaseCodeEquipment(object sender, EventArgs e)
		{
			var picker = (Picker)((StackLayout)sender).Parent.FindByName("pk_eq_Phasecode");
			picker.Focus();
		}
		private void Arrow_OnTapped_JobCategory(object sender, EventArgs e)
		{
			var picker = (Picker)((Image)sender).Parent.FindByName("pk_JobCategoryCode");
			picker.Focus();
		}
		private void Arrow_OnTapped_JobCategoryEquipment(object sender, EventArgs e)
		{
			var picker = (Picker)((StackLayout)sender).Parent.FindByName("pk_eq_JobCategoryCode"); ;
			picker.Focus();
		}
		private void ic_addEmp_Clicked(object sender, EventArgs e)
		{
			var buttonView = (StackLayout)((ImageButton)sender).Parent.FindByName("empEqpButton");
			if (buttonView.IsVisible)
			{
				((ImageButton)sender).Source = ImageSource.FromResource("Nucleus.UI.Assets.Images.AddEmployees.png");
				buttonView.IsVisible = false;
			}
			else
			{
				((ImageButton)sender).Source = ImageSource.FromResource("Nucleus.UI.Assets.Images.remove.png");
				buttonView.IsVisible = true;
			}
			var vm = BindingContext as TimesheetPerJobViewModel;
			if (vm.Employees.Count() <= 0)
			{
				bg_reservationEmp.IsVisible = true;
				lv_resourcePopUp.IsVisible = false;
			}
			else
			{ bg_reservationEmp.IsVisible = false;
				lv_resourcePopUp.IsVisible = true;
			}
			if (vm.Equipment.Count() <= 0)
			{
				bg_reservationEqp.IsVisible = true;
				lv_EquipmentPopUp.IsVisible = false;
			}
			else
			{
				bg_reservationEqp.IsVisible = false;
				lv_EquipmentPopUp.IsVisible = true;
			}
			
		}

		private void Button_Clicked(object sender, EventArgs e)
		{
			var listView = (AbsoluteLayout)((Button)sender).Parent.Parent.FindByName("listheaderEmp");
			listView.IsVisible = false;
			var stack = (StackLayout)((Button)sender).Parent.Parent.Parent.FindByName("empEqpButton");
			var imgButton = (ImageButton)((Button)sender).Parent.Parent.Parent.FindByName("ic_addEmp");
			imgButton.Source= ImageSource.FromResource("Nucleus.UI.Assets.Images.AddEmployees.png");
			stack.IsVisible = false;
			
		}
		private void Button_Clicked_closeEqupPopup(object sender, EventArgs e)
		{
			var listView = (AbsoluteLayout)((Button)sender).Parent.Parent.FindByName("listheader");
			listView.IsVisible = false;
			var stack = (StackLayout)((Button)sender).Parent.Parent.Parent.FindByName("empEqpButton");
			var imgButton = (ImageButton)((Button)sender).Parent.Parent.Parent.FindByName("ic_addEmp");
			imgButton.Source = ImageSource.FromResource("Nucleus.UI.Assets.Images.AddEmployees.png");
			stack.IsVisible = false;
		}

		private void Button_Clicked_ResourceEmp(object sender, EventArgs e)
		{
			var type = sender.GetType().FullName;
			if (type.Equals("Xamarin.Forms.ImageButton"))
			{
				var listView = (AbsoluteLayout)((ImageButton)sender).FindByName("listheaderEmp");	
				listView.IsVisible = true;
				var lv = (ListView)((ImageButton)sender).FindByName("lv_resourcePopUp");
				var bm = BindingContext as TimesheetPerJobViewModel;
				lv.ItemsSource = bm.Employees;
			}
			else
			{
				var listView1 = (StackLayout)((Button)sender).FindByName("lv_resourcePopUp");
				listView1.IsVisible = true;
				var lv = (ListView)((ImageButton)sender).FindByName("lv_resourcePopUp");
				var bm = BindingContext as TimesheetPerJobViewModel;
				lv.ItemsSource = bm.Employees;

			}
		}
		private void Button_Clicked_ResourceEquip(object sender, EventArgs e)
		{
			var type = sender.GetType().FullName;
			if (type.Equals("Xamarin.Forms.ImageButton"))
			{
				var listView = (AbsoluteLayout)((ImageButton)sender).Parent.FindByName("listheader");
				listView.IsVisible = true;
				var lv = (ListView)((ImageButton)sender).FindByName("lv_EquipmentPopUp");
				var bm = BindingContext as TimesheetPerJobViewModel;
				lv.ItemsSource = bm.Equipment;
			}
			else
			{
				var listView1 = (AbsoluteLayout)((Button)sender).Parent.FindByName("listheader");
				listView1.IsVisible = true;
				var lv = (ListView)((ImageButton)sender).FindByName("lv_EquipmentPopUp");
				var bm = BindingContext as TimesheetPerJobViewModel;
				lv.ItemsSource = bm.Equipment;
			}
		}

		private async void chk_CheckedChanged(object sender, CheckedChangedEventArgs e)
		{
			var checkbox = (CheckBox)sender;
			var ob = checkbox.BindingContext;
			var vm = BindingContext as TimesheetPerJobViewModel;
			if (checkbox.IsChecked)
			{
				try
				{
					vm.PopulateEmployeeGrid.Execute(ob);
				} catch (Exception ex)
				{
					await DisplayAlert("Error", ex.Message.ToString(), "OK");
				}
			}

			if (!checkbox.IsChecked)
			{

				try
				{
					var result = await DisplayAlert("Remove Employee Detail", "The selected Employee and existing time entries will be removed from Timesheet. Are you sure you would like to remove this employee and their time entries?", "Confirm", "Cancel");
					if (result.Equals(true))
					{
						vm.RemoveItemfromEmployeeGrid.Execute(ob);
                    }
                    else
                    {
						checkbox.IsChecked = true;

					}
				}
				catch (Exception ex)
				{
					await DisplayAlert("Error", ex.Message.ToString(), "OK");
				}
			}
			if (vm.TimesheetLineItems.Count() <= 0)
				bg_timesheet.IsVisible = true;
			else
				bg_timesheet.IsVisible = false;
			if (vm.Employees.Count() <= 0)
			{
				bg_reservationEmp.IsVisible = true;
				lv_resourcePopUp.IsVisible = false;
			}
			else
			{
				bg_reservationEmp.IsVisible = false;
				lv_resourcePopUp.IsVisible = true;
			}

			if (vm.Equipment.Count() <= 0)
			{
				bg_reservationEqp.IsVisible = true;
				lv_EquipmentPopUp.IsVisible = false;

			}
			else
			{
				bg_reservationEqp.IsVisible = false;
				lv_EquipmentPopUp.IsVisible = true;
			}

		}
		private async void chkEqup_CheckedChanged_Equipment(object sender, CheckedChangedEventArgs e)
		{
			var checkbox = (CheckBox)sender;
			var ob = checkbox.BindingContext;
			var vm = BindingContext as TimesheetPerJobViewModel;
			if (checkbox.IsChecked)
				vm.PopulateEquipmentGrid.Execute(ob);
			if (!checkbox.IsChecked)
            {
				var result = await DisplayAlert("Remove Equipment Detail", "The selected Equipment and existing time entries will be removed from timesheet are you sure you want to delete entry?", "Confirm", "Cancel");
				if (result.Equals(true))
				{
					vm.RemoveItemfromEquipmentGrid.Execute(ob);
				}
				else
				{
					checkbox.IsChecked = true;

				}
			}
				
			if (vm.TimesheetLineItems.Count() <= 0)
			{
				bg_timesheet.IsVisible = true;

			}
			else
			{
				bg_timesheet.IsVisible = false;
			}
			
			if (vm.Equipment.Count() <= 0)
			{
				bg_reservationEqp.IsVisible = true;
				lv_EquipmentPopUp.IsVisible = false;
			}
			else
			{
				bg_reservationEqp.IsVisible = false;
				lv_EquipmentPopUp.IsVisible = true;
			}
		}

		private void lb_Description_Clicked(object sender, EventArgs e)
		{
			var lv = (ListView)((Button)sender).Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.FindByName("lv_descriptionPopUp");
			var btn = (Button)sender;
			btn_description = btn;
			lv.IsVisible = true;
		}

		private void Button_Clicked_lb_headerLabel(object sender, EventArgs e)
		{
			var listView = (ListView)((Button)sender).Parent.Parent.FindByName("lv_descriptionPopUp");
			listView.IsVisible = false;

		}


		private async void ToolbarItem_Clicked(object sender, EventArgs e)
		{
			var vm = BindingContext as TimesheetPerJobViewModel;
			var button = (ToolbarItem)sender;
			if(vm.selectedcount == 0)
            {
				var result = "in Process";
				var buttonContext = button.BindingContext;
				var tm = vm.TimesheetLineItems.Where(a => ((a.IsValid.Equals(true) || a.IsPaytype2Valid.Equals(true) || a.IsPaytype3Valid.Equals(true) ) && (a.Type.Equals("Employees")))|| a.IsPerDimValid.Equals(true) || a.IsMiscValid.Equals(true));
				if (tm.Count() > 0)
					DisplayAlert("Invalid Entry", "Please Correct Invalid Entry to Save the Timesheet", "Ok");
				else
				{
					if (vm.TimesheetLineItems.Where(a => a.Type.Equals("Equipment") && a.EquipmentUsage.IsNullOrEmpty()).Count() > 0)
						showError();
					else if (vm.TimesheetLineItems.Where(a => a.Type.Equals("Employees") && (a.Paytype1Text.IsNullOrEmpty() || a.Paytype2Text.IsNullOrEmpty() || a.Paytype3Text.IsNullOrEmpty() || a.PerDimText.IsNullOrEmpty() || a.MiscText.IsNullOrEmpty())).Count() > 0)
						showError();
					else
					{
						foreach (TimesheetPerJobListModel dto in vm.TimesheetLineItems)
						{
							if (Convert.ToString(dto.Section1).IsNullOrEmpty())
							{
								dto.Section1 = vm.PayTypes1.First();
							}
							if (Convert.ToString(dto.Section2).IsNullOrEmpty())
							{
								dto.Section2 = vm.PayTypes2.First();
							}
							if (Convert.ToString(dto.Section3).IsNullOrEmpty())
							{
								dto.Section3 = vm.PayTypes3.First();
							}
							//if (dto.Type.Equals("Equipment") && (dto.EquipmentUsage.IsNullOrEmpty() || dto.EquipmentUsage.Equals(0)))
							//{ showError(); break; }
							//if (dto.Type.Equals("Employees") && (dto.Paytype1Text.IsNullOrEmpty() || dto.Paytype2Text.IsNullOrEmpty() || dto.Paytype3Text.IsNullOrEmpty() || dto.PerDimText.IsNullOrEmpty() || dto.MiscText.IsNullOrEmpty()) || (dto.Paytype1Text.Equals("0") && dto.Paytype2Text.Equals("0") && dto.Paytype3Text.Equals("0") && dto.PerDimText.Equals("0") && dto.MiscText.Equals("0")))
							//{ showError(); break; }
							//else
							//{
								result = vm.SaveTimesheetPerJob(dto);
						//	}

						}
					}
                    try
                    {
						if (vm.TimesheetLineItems.Count() > 0 && !result.Equals("in Process"))
						{
							await DisplayAlert("Timesheet Saved", result, "Ok");
							await Navigation.PopAsync();
						}
					}
                    catch (Exception ex)
                    {

                        throw ex;
                    }
					
				}
            }
            else
            {
				Entertime.IsVisible = true;
				Entertime1.IsVisible = true;
				vm.selected_time = "";
			}
			
		}

		void showError()
        {
			DisplayAlert("Unable to Save", "Please make sure timesheet entries are filled out.", "Ok");
		}
		private void pk_paytypeEntry1_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				var regex = new Regex(@"^[0-9]{1}(\.[0-9]{0,1}[0-9]{0,1})?$|^[1-2]{1}[0-3]{1}(\.[0-9]{0,1}[0-9]{0,1})?$|^24(\.[0]{0,1}[0]{0,1})?$");
				if (!regex.IsMatch(e.NewTextValue.ToString()))
				{
					DisplayAlert(e.NewTextValue.ToString(), "Invalid Entry", "Ok");
					((Entry)sender).Text = e.OldTextValue;
				}
			} catch (Exception ex) {
				DisplayAlert("Error", ex.Message.ToString(), "OK");
			}
			
		}

        /*private void screen_double_tap(object sender, EventArgs e)
        {
			var vm = BindingContext as TimesheetPerJobViewModel;
			if (!vm.selectenabled)
            {
				
				vm.Toolbar_name = "Edit";
				vm.selectenabled = true;
				foreach (var res in vm.TimesheetLineItems)
					res.isEnabled = false;

			}
            else
            {
				vm.Toolbar_name = "Save";
				vm.selectenabled = false;
				foreach (var res in vm.TimesheetLineItems)
				{
					res.isEnabled = true;
					res.miscColor = Color.White;
					res.paytype1Color = Color.White;
					res.paytype2Color = Color.White;
					res.paytype3Color = Color.White;
					res.perDimColor = Color.White;
					res.UsageColor = Color.White;
				}
			}
			
		}*/

        private void Time_selected(object sender, EventArgs e)
        {
			var vm = BindingContext as TimesheetPerJobViewModel;
			foreach(var res in vm.TimesheetLineItems)
            {
				if (res.miscColor == Color.Gray)
					res.MiscText = vm.selected_time;
				if (res.paytype1Color == Color.Gray)
					res.Paytype1Text = vm.selected_time;
				if (res.paytype2Color == Color.Gray)
					res.Paytype2Text = vm.selected_time;
				if (res.paytype3Color == Color.Gray)
					res.Paytype3Text = vm.selected_time;
				if (res.perDimColor == Color.Gray)
					res.PerDimText = vm.selected_time;
				if (res.UsageColor == Color.Gray)
					res.EquipmentUsage = vm.selected_time;
				res.UsageEnabled = true;
				res.paytype1Enabled = true;
				res.paytype2Enabled = true;
				res.paytype3Enabled = true;
				res.perdimEnabled = true;
				res.miscEnabled = true;
				res.miscColor = Color.White;
				res.paytype1Color = Color.White;
				res.paytype2Color = Color.White;
				res.paytype3Color = Color.White;
				res.perDimColor = Color.White;
				res.UsageColor = Color.White;
			}
			vm.Toolbar_name = "save";
			vm.selectedcount = 0;
			Entertime.IsVisible = false;
			Entertime1.IsVisible = false;
			vm.selected_time = string.Empty;
		}

        private void Entertime_tapped(object sender, EventArgs e)
        {
			var vm = BindingContext as TimesheetPerJobViewModel;
			Entertime.IsVisible = false;
			Entertime1.IsVisible = false;
			vm.selected_time = string.Empty;
		}
        
		private void selected_tap_usage(object sender, EventArgs e)
		{
			//var entry = (Entry)((StackLayout)sender).FindByName("usageentry");
			var vm = BindingContext as TimesheetPerJobViewModel;
			vm.selected_UsageCommandAsync((int)(((TappedEventArgs)e).Parameter));
			//entry.IsEnabled = entry.IsEnabled ? false : true;
		}

		private void selected_tap_paytype1(object sender, EventArgs e)
		{
			//var entry = (Entry)((StackLayout)sender).FindByName("pk_paytypeEntry1");
			var vm = BindingContext as TimesheetPerJobViewModel;
			vm.selected_paytype1CommandAsync((int)(((TappedEventArgs)e).Parameter));
			//entry.IsEnabled = entry.IsEnabled ? false : true;
		}

		private void selected_tap_paytype2(object sender, EventArgs e)
		{
			//var entry = (Entry)((StackLayout)sender).FindByName("pk_paytypeEntry2");
			var vm = BindingContext as TimesheetPerJobViewModel;
			vm.selected_paytype2CommandAsync((int)(((TappedEventArgs)e).Parameter));
			//entry.IsEnabled = entry.IsEnabled ? false : true;
		}

		private void selected_tap_paytype3(object sender, EventArgs e)
		{
			//var entry = (Entry)((StackLayout)sender).FindByName("pk_paytypeEntry3");
			var vm = BindingContext as TimesheetPerJobViewModel;
			vm.selected_paytype3CommandAsync((int)(((TappedEventArgs)e).Parameter));
			//entry.IsEnabled = entry.IsEnabled ? false : true;
		}

		private void selected_tap_PerDim(object sender, EventArgs e)
		{
			//var entry = (Entry)((StackLayout)sender).FindByName("perdimentry");
			var vm = BindingContext as TimesheetPerJobViewModel;
			vm.selected_perDimCommandAsync((int)(((TappedEventArgs)e).Parameter));
			//entry.IsEnabled = entry.IsEnabled ? false : true;
		}

		private void selected_tap_misc(object sender, EventArgs e)
		{
			//var entry = (Entry)((StackLayout)sender).FindByName("miscentry");
			var vm = BindingContext as TimesheetPerJobViewModel;
			vm.selected_miscCommandAsync((int)(((TappedEventArgs)e).Parameter));
			//entry.IsEnabled = entry.IsEnabled ? false : true;
		}

        private void lv_descriptionPopUp_ItemTapped(object sender, ItemTappedEventArgs e)
        {
			var lv = (ListView)sender;
			var ob = lv.BindingContext;
			var vm = BindingContext as TimesheetPerJobViewModel;
			vm.selectedDescription = ((ExpenseTypesDto)e.Item).Name.ToString();
			btn_description.Text = ((ExpenseTypesDto)e.Item).Name.ToString();
			var btmvm = (TimesheetPerJobListModel)btn_description.BindingContext;
			vm.SelectedDescription(btmvm, btn_description.Text);
			lv.IsVisible = false;
		}
    }
}