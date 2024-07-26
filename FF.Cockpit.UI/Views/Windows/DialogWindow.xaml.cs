using FF.Cockpit.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FF.Cockpit.UI.Views.Windows
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public enum MsgBoxImage 
    { 
        Asterisk,
      Error,
      Exclamation, 
      Hand, 
      Information, None, Question, Stop, Warning
}
public enum MsgBoxResult { Cancel, No, None, Keep, OK, Return, Yes, Delete, Unassign, Add, Reset, Suspend, CancelTransaction, Continue, Deliver }

    public enum MsgBoxButton
    {
        OK, OKCancel, KeepReturnCancel, YesNo, YesNoCancel, DeleteUnassignCancel, AddReset, SuspendContinueCancel, AddtoqueueContinueCancel, DeliverContinueCancel, SuspendTill
    }
    public partial class DialogWindow : Window
    {
        private MsgBoxButton _msgBoxButton = MsgBoxButton.OKCancel;
        private MsgBoxResult _msgBoxResult = MsgBoxResult.Cancel;
        private MsgBoxImage _msgBoxImage = MsgBoxImage.None;
        public DialogWindow()
        {
            InitializeComponent();
        }

        public MsgBoxButton MsgBoxButton
        {
            get
            {
                return _msgBoxButton;
            }
            set
            {
                _msgBoxButton = value;

                switch (value)
                {
                    case MsgBoxButton.OK:
                        buttonOk.IsDefault = true;
                        buttonOk.Visibility = Visibility.Visible;
                        buttonYes.Visibility = Visibility.Collapsed;
                        buttonNo.Visibility = Visibility.Collapsed;
                        buttonCancel.Visibility = Visibility.Collapsed;

                        break;
                    case MsgBoxButton.OKCancel:
                        buttonCancel.IsDefault = true;
                        buttonOk.Visibility = Visibility.Visible;
                        buttonOk.Visibility = Visibility.Visible;
                        buttonYes.Visibility = Visibility.Collapsed;
                        buttonNo.Visibility = Visibility.Collapsed;

                        break;
                    case MsgBoxButton.YesNo:
                        buttonNo.IsDefault = true;
                        buttonOk.Visibility = Visibility.Collapsed;
                        buttonCancel.Visibility = Visibility.Collapsed;
                        buttonYes.Visibility = Visibility.Visible;
                        buttonNo.Visibility = Visibility.Visible;

                        break;
                    case MsgBoxButton.YesNoCancel:
                        buttonCancel.IsDefault = true;
                        buttonOk.Visibility = Visibility.Collapsed;
                        buttonCancel.Visibility = Visibility.Visible;
                        buttonYes.Visibility = Visibility.Visible;
                        buttonNo.Visibility = Visibility.Visible;

                        break;

                }
            }
        }

        public MsgBoxResult MsgBoxResult
        {
            get
            {
                return _msgBoxResult;
            }
            set
            {
                _msgBoxResult = value;
            }
        }

        public MsgBoxImage MsgBoxImage
        {
            get
            {
                return _msgBoxImage;
            }
            set
            {
                _msgBoxImage = value;
                switch (value)
                {
                    //case MsgBoxImage.Information:
                    //    successImage.Visibility = Visibility.Visible;
                    //    successImage1.Visibility = Visibility.Visible;
                    //    break;

                    case MsgBoxImage.Warning:
                        alertImage.Visibility = Visibility.Visible;
                        break;
                }
            }
        }
        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MsgBoxResult = MsgBoxResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }


        public void buttonYes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MsgBoxResult = MsgBoxResult.Yes;
                this.Close();
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }

        private void buttonNo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MsgBoxResult = MsgBoxResult.No;
                this.Close();
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }


        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MsgBoxResult = MsgBoxResult.Cancel;
                this.Close();
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }

        private void buttonShowHideDetailtxt_Click(object sender, RoutedEventArgs e)
        {
            if (((ToggleButton)sender).IsChecked == true)
            {
               
                errorMessageText.Visibility = Visibility.Visible;
            }
            else
            {
                
                errorMessageText.Visibility = Visibility.Collapsed;
            }
        }
       
    }
}
