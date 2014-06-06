using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using CompassPlus.ViewModels;

namespace CompassPlus.Views
{
    public partial class AboutView : PhoneApplicationPage
    {
        public AboutView()
        {
            InitializeComponent();

            this.DataContext = new AboutViewModel();
        }
    }
}