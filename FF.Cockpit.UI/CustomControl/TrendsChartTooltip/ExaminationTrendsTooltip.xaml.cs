using LiveCharts.Wpf;
using LiveCharts;
using System.ComponentModel;

namespace FF.Cockpit.UI.CustomControl
{
    /// <summary>
    /// Interaction logic for ExaminationTrendsTooltip.xaml
    /// </summary>
    public partial class ExaminationTrendsTooltip : IChartTooltip
    {
        private TooltipData _data;

        public ExaminationTrendsTooltip()
        {
            InitializeComponent();

            //LiveCharts will inject the tooltip data in the Data property
            //your job is only to display this data as required
            Data = new TooltipData() { };
            
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public TooltipData Data
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }

        public TooltipSelectionMode? SelectionMode { get; set; }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
