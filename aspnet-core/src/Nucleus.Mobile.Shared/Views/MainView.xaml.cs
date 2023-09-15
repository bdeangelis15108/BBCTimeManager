using Xamarin.Forms;

namespace Nucleus.Views
{
    public partial class MainView : MasterDetailPage, IXamarinView
    {
        public MainView()
        {
            InitializeComponent();
           NavigationPage.SetHasNavigationBar(this, false);
            
        }

       
    }
}
