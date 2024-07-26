using LiveCharts.Wpf;
using LiveCharts;
using System.ComponentModel;

namespace FF.Cockpit.UI.CustomControl
{
    /// <summary>
    /// Interaction logic for PatientTrendsTooltip.xaml
    /// </summary>
    public partial class PatientTrendsTooltip : IChartTooltip
    {
        private TooltipData _data;

        public PatientTrendsTooltip()
        {
            InitializeComponent();
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
