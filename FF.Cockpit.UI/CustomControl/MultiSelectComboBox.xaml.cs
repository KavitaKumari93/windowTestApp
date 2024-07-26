using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FF.Cockpit.UI.CustomControl
{
    /// <summary>
    /// Interaction logic for MultiSelectComboBox.xaml
    /// </summary>
    public partial class MultiSelectComboBox : UserControl
    {
        private ObservableCollection<Node> _nodeObservableCollection;
        public MultiSelectComboBox()
        {
            InitializeComponent();
            _nodeObservableCollection = new ObservableCollection<Node>();
        }

        #region Dependency Properties

        public static readonly DependencyProperty ItemsSourceProperty =
             DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<string>), typeof(MultiSelectComboBox), new FrameworkPropertyMetadata(null,
        new PropertyChangedCallback(MultiSelectComboBox.OnItemsSourceChanged)));

        public static readonly DependencyProperty SelectedItemsProperty =
         DependencyProperty.Register("SelectedItems", typeof(ObservableCollection<string>), typeof(MultiSelectComboBox), new FrameworkPropertyMetadata(null,
     new PropertyChangedCallback(MultiSelectComboBox.OnSelectedItemsChanged)));

        public static readonly DependencyProperty TextProperty =
           DependencyProperty.Register("Text", typeof(string), typeof(MultiSelectComboBox), new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty DefaultTextProperty =
            DependencyProperty.Register("DefaultText", typeof(string), typeof(MultiSelectComboBox), new UIPropertyMetadata(string.Empty));



        public ObservableCollection<string> ItemsSource
        {
            get { return (ObservableCollection<string>)GetValue(ItemsSourceProperty); }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        public ObservableCollection<string> SelectedItems
        {
            get { return (ObservableCollection<string>)GetValue(SelectedItemsProperty); }
            set
            {
                SetValue(SelectedItemsProperty, value);
            }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string DefaultText
        {
            get { return (string)GetValue(DefaultTextProperty); }
            set { SetValue(DefaultTextProperty, value); }
        }
        #endregion

        #region Events
        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSelectComboBox control = (MultiSelectComboBox)d;
            control.DisplayInControl();
        }

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSelectComboBox control = (MultiSelectComboBox)d;
            control.SelectNodes();
            control.SetText();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox clickedBox = (CheckBox)sender;
            int _selectedCount = 0;
            foreach (Node s in _nodeObservableCollection)
            {
                if (s.IsSelected)
                    _selectedCount++;
            }
            SetSelectedItems();
            SetText();
        }
        #endregion


        #region Methods
        private void SelectNodes()
        {
            if (SelectedItems != null)
            {
                foreach (var keyValue in SelectedItems)
                {
                    Node node = _nodeObservableCollection.FirstOrDefault(i => i.Title == keyValue);
                    if (node != null)
                        node.IsSelected = true;
                }
            }
        }

        private void SetSelectedItems()
        {
            if (SelectedItems == null)
                SelectedItems = new ObservableCollection<string>();
            SelectedItems.Clear();
            foreach (Node node in _nodeObservableCollection)
            {
                if (node.IsSelected && node.Title != "All")
                {
                    if (this.ItemsSource.Count > 0)

                        SelectedItems.Add(node.Title);
                }
            }
        }

        private void DisplayInControl()
        {
            _nodeObservableCollection.Clear();
            //if (this.ItemsSource.Count > 0)
            //    _nodeObservableCollection.Add(new Node("All"));
            foreach (var keyValue in this.ItemsSource)
            {
                Node node = new Node(keyValue);
                _nodeObservableCollection.Add(node);
            }
            MultiSelectCombo.ItemsSource = _nodeObservableCollection;
        }

        private void SetText()
        {
            if (this.SelectedItems != null)
            {
                StringBuilder displayText = new StringBuilder();
                foreach (Node s in _nodeObservableCollection)
                {
                    if (s.IsSelected == true && s.Title == "All")
                    {
                        displayText = new StringBuilder();
                        displayText.Append("All");
                        break;
                    }
                    else if (s.IsSelected == true && s.Title != "All")
                    {
                        displayText.Append(s.Title);
                        displayText.Append(',');
                    }
                }
                this.Text = displayText.ToString().TrimEnd(new char[] { ',' }); 
            }           
            // set DefaultText if nothing else selected
            if (string.IsNullOrEmpty(this.Text))
            {
                this.Text = this.DefaultText;
            }
        }

       
        #endregion
    }

    public class Node : INotifyPropertyChanged
    {

        private string _title;
        private bool _isSelected;
        #region ctor
        public Node(string title)
        {
            Title = title;
        }
        #endregion

        #region Properties
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                NotifyPropertyChanged("Title");
            }
        }
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                NotifyPropertyChanged("IsSelected");
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
