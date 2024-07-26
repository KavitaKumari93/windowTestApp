﻿using FF.Cockpit.Entity.EntityModel;
using FF.Cockpit.Entity.StoreProcedure_Result;
using LiveCharts;
using LiveCharts.Configurations;
using System.Windows.Controls;
using System.Windows.Media;

namespace FF.Cockpit.UI.Views.UserControls.TrendsGraphExamined
{
    /// <summary>
    /// Interaction logic for ExaminationTrends_20Year.xaml
    /// </summary>
    public partial class ExaminationTrends_20Year : UserControl
    {
        public ExaminationTrends_20Year()
        {
            InitializeComponent();
            //var mapper = Mappers.Pie<PointLable>().Value(x => x.Value);
           // Charting.For<PointLable>(mapper);
            
            ////create the mapper
            //var mapper = new CartesianMapper<ExaminationTrendsData_Result>()
            //    .X((value, index) => index)
            //    .Y((value) => value.DermoscopeSessionCount)
            //    .Fill((value, index) => (value.DermoscopeSessionCount == 0 ? Brushes.Red : Brushes.White));
            //Charting.For<ExaminationTrendsData_Result>(mapper);

            ////assign the mapper globally (!)
            //LiveCharts.Charting.For<double>(mapper, SeriesOrientation.Horizontal);

            //new LineSeriesProps<double>
            //{
            //    DataLabelsSize = 20,
            //    DataLabelsPaint = new   (SKColors.Blue),
            //    // all the available positions at:
            //    // https://lvcharts.com/api/2.0.0-beta.220/LiveChartsCore.Measure.DataLabelsPosition
            //    DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top,
            //    // The DataLabelsFormatter is a function 
            //    // that takes the current point as parameter
            //    // and returns a string.
            //    // in this case we returned the PrimaryValue property as currency
            //    DataLabelsFormatter = (point) => point.PrimaryValue.ToString("C2"),
            //    Values = new ObservableCollection<double> { 2, 1, 3, 5, 3, 4, 6 },
            //    Fill = null
            //}
        }
    }
}
