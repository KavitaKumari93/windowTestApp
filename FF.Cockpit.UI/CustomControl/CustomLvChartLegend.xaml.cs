using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using localisation=FF.Cockpit.Common.Properties.Resources;

namespace FF.Cockpit.UI.CustomControl
{
    public partial class CustomLvChartLegend : UserControl, IChartLegend
    {
        /// <summary>
        /// Orientation of the legend entries
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(CustomLvChartLegend), new PropertyMetadata(Orientation.Horizontal));


        public CustomLvChartLegend()
        {
            InitializeComponent();

            itemsControl.DataContext = this;
        }

        public ObservableCollection<CustomSeriesViewModel> LegendEntries { get; } = new ObservableCollection<CustomSeriesViewModel>();

        public List<SeriesViewModel> Series
        {
            get => LegendEntries.Select(x => x.SeriesViewModel).ToList();
            set
            {
                Chart ownerChart = GetOwnerChart();

                // note: value only contains the visible series                
                // remove all entries which also have been removed from the chart
                var removedSeries = LegendEntries.Where(x => !ownerChart.Series.Any(s => s == x.View)).ToList();
                foreach (var rs in removedSeries)
                    LegendEntries.Remove(rs);

                foreach (var svm in value)
                {
                    // add entries which are new                                        
                    // The SeriesViewModel instances are always new, so we have to compare using the title

                    if (!LegendEntries.Any(x => x.Title == svm.Title))
                    {
                        // find the series' UIElement by title

                        //svm.Title = svm.Fill.ToString().Contains("8E7358") ? "Dermo" : "Body";
                        ownerChart.Series.FirstOrDefault(x => x.IsSeriesVisible);
                        //var dd = ((FrameworkElement)(<ISeriesView>(ownerChart.Series).Items[0])).Name;
                        LineSeries seriesView = ownerChart.Series.FirstOrDefault(x => x.Title == svm.Title) as LineSeries;
                        if (seriesView != null)
                        {
                            svm.Title = seriesView.Title = seriesView.Name == "DermoscopeSeries" ? localisation.DermoscopyStr_resx : localisation.BodyscanStr_resx;
                        }
                        LegendEntries.Add(new CustomSeriesViewModel(svm, seriesView));
                    }
                }


                OnPropertyChanged();
            }
        }

        private Chart GetOwnerChart()
        {
            return FindParent<Chart>(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }
    }

    public class CustomSeriesViewModel : INotifyPropertyChanged
    {
        public string Title { get => SeriesViewModel.Title; }

        public Brush Fill { get => SeriesViewModel.Fill ?? SeriesViewModel.Stroke; }

        public SeriesViewModel SeriesViewModel { get; }

        public ISeriesView View { get; }

        public bool IsVisible
        {
            get => ((UIElement)View).Visibility == Visibility.Visible;
            set
            {
                if (IsVisible != value)
                {
                    ((UIElement)View).Visibility = value ? Visibility.Visible : Visibility.Hidden;

                    OnPropertyChanged();
                }
            }
        }

        public CustomSeriesViewModel(SeriesViewModel svm, ISeriesView view)
        {
            this.SeriesViewModel = svm;
            this.View = view;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
