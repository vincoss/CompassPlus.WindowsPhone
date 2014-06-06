using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using CompassPlus.Sensors;
using CompassPlus.Services;
using CompassPlus.ViewModels;
using CompassPlus.Globalization;


namespace CompassPlus.Views
{
    public partial class CompassView : PhoneApplicationPage
    { 
        public CompassView()
        {
            InitializeComponent();
        }

        #region Event handlers

        private void CompassView_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            // Switch the placement of the buttons based on an orientation change.
            if ((e.Orientation & PageOrientation.Portrait) == (PageOrientation.Portrait))
            {
                Grid.SetRow(GridProperties, 1);
                Grid.SetColumn(GridProperties, 0);
            }
            // If not in portrait, move buttonList content to visible row and column.
            else
            {
                Grid.SetRow(GridProperties, 0);
                Grid.SetColumn(GridProperties, 1);
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs args)
        {
            Render(args.NewSize.Height);
        }

        private void Render(double height)
        {
            var model = (CompassRoseViewModel)this.DataContext;
            if (model == null)
            {
                return;
            }

            var size = LayoutRoot.ActualWidth;
            if (this.Orientation == PageOrientation.Landscape || this.Orientation == PageOrientation.LandscapeLeft || this.Orientation == PageOrientation.LandscapeRight)
            {
                size = height;
            }

            model.Scale = (size) / 128;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.SizeChanged -= OnSizeChanged;
            this.OrientationChanged -= CompassView_OrientationChanged;

            this.SizeChanged += OnSizeChanged;
            this.OrientationChanged += CompassView_OrientationChanged;

            var model = new CompassRoseViewModel(new DeviceCompass(), new IsolatedSettings(), new MessageService());
            model.UiAction = (uiAction => this.Dispatcher.BeginInvoke(uiAction));

            this.DataContext = model;

            BuildLocalizedApplicationBar();
            ViewModel.Initialize();
            ViewModel.Start();

            // On hibernante we need to update layout since SizeChanged does not fire
            Render(LayoutRoot.ActualHeight);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            ViewModel.Stop();
        }

        private void ApplicationBarSettingsMenuItem_OnClick(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Views/SettingsView.xaml", UriKind.Relative));
        }

        private void ApplicationBarAboutMenuItem_OnClick(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Views/AboutView.xaml", UriKind.Relative));
        }

        #endregion

        #region Private methods

        private void BuildLocalizedApplicationBar()
        {
            var applicationBar = new ApplicationBar();
            applicationBar.IsMenuEnabled = true;
            applicationBar.Mode = ApplicationBarMode.Minimized;

            ApplicationBar = applicationBar;

            // Settings
            var item = new ApplicationBarMenuItem();
            item.Text = new LocalizedStrings()["Settings"];
            item.Click += ApplicationBarSettingsMenuItem_OnClick;

            ApplicationBar.MenuItems.Add(item);

            item = new ApplicationBarMenuItem();
            item.Text = new LocalizedStrings()["About"];
            item.Click += ApplicationBarAboutMenuItem_OnClick;

            ApplicationBar.MenuItems.Add(item);
        }

        private CompassRoseViewModel ViewModel
        {
            get { return (CompassRoseViewModel)DataContext; }
        } 

        #endregion

    }
}