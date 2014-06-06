using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using CompassPlus.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace CompassPlus.Views
{
    // TODO: Nice to have
    public partial class CalibrateView : PhoneApplicationPage
    {
        public CalibrateView()
        {
            InitializeComponent();

            this.DataContext = new CalibrateViewModel();
        }
    }
}