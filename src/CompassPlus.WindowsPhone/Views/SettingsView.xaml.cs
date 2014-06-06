using Microsoft.Phone.Controls;

using CompassPlus.Services;
using CompassPlus.ViewModels;


namespace CompassPlus.Views
{
    public partial class SettingsView : PhoneApplicationPage
    {
        public SettingsView()
        {
            InitializeComponent();

            var model = new SettingsViewModel(new IsolatedSettings(), new MessageService());

            this.DataContext = model;

            model.Initialize();
        }
    }
}