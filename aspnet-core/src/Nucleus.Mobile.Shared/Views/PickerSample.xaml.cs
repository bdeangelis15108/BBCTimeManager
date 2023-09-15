using System.Collections.Generic;
using Xamarin.Forms;

namespace Nucleus.Views
{
	public partial class PickerSample : ContentPage, IXamarinView
    {
		public IList<string> weight_units = new List<string>();

		public PickerSample()
		{
			InitializeComponent ();
			weight_units.Add("lbs");
			weight_units.Add("kgs");
			string weight_unit = "lbs";
			lv_Pickersample.ItemsSource = new string[] { "1", "2" };
		
			BindingContext = new { Wunits = weight_units, Wunit = weight_unit };

		}

		
	}
}