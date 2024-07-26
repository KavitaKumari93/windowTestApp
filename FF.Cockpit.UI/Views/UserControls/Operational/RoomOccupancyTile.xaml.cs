﻿using System;
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

namespace FF.Cockpit.UI.Views.UserControls.Operational
{
    /// <summary>
    /// Interaction logic for RoomOccupancyTile.xaml
    /// </summary>
    public partial class RoomOccupancyTile : UserControl
    {
        public RoomOccupancyTile()
        {
            InitializeComponent();
            var yAxis = barChart.AxisY[0];
            yAxis.MaxValue = 100;
            yAxis.MinValue = 0;
            yAxis.LabelFormatter = value => $"{value:0}%";
        }
    }
}
