﻿using FF.Cockpit.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FF.Cockpit.UI.Views.UserControls.Configuration
{
    /// <summary>
    /// Interaction logic for RoleConfigurationView.xaml
    /// </summary>
    public partial class RoleConfigurationView : UserControl
    {
        public RoleConfigurationView(ConfigurationViewModel vm)
        {
            InitializeComponent();
            this.DataContext = vm;

        }
    }
}
