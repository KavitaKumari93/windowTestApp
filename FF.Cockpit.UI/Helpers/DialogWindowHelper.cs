using FF.Cockpit.Common;
using FF.Cockpit.UI.Views.Windows;
using System;
using System.Windows;

namespace FF.Cockpit.UI.Helpers
{
    public class DialogWindowHelper
    {
        public  MsgBoxResult Show(string title, string caption, string confirmationText, string message, MsgBoxButton button, MsgBoxImage icon)
        {
            Views.Windows.DialogWindow dialogWindow = new Views.Windows.DialogWindow();
            try
            {
                dialogWindow.txt_title.Text = title;
                dialogWindow.txt_title_caption.Text = caption;
                dialogWindow.confirmationText.Text = confirmationText;
                string[] stringMessage = message.Split('.');
                dialogWindow.textmessage.Text = message;
                dialogWindow.MsgBoxButton = button;
                dialogWindow.MsgBoxImage = icon;
                dialogWindow.errorMessageText.Visibility = Visibility.Collapsed;
                dialogWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
            return dialogWindow.MsgBoxResult;
        }
        public bool ShowPopupMessage(string title, string caption, string confirmationText,string messageContent, bool isConfirmation, bool isWarning = false)
        {
            bool dialogResult = false;
            MsgBoxImage msgBoxImage = MsgBoxImage.Exclamation;

            if (isWarning)
                msgBoxImage = MsgBoxImage.Warning;
            MsgBoxResult result = MsgBoxResult.No;
            if (!isConfirmation)
                result = Show(title, caption, confirmationText,messageContent, MsgBoxButton.OK, msgBoxImage);
            else if (isConfirmation)
                result = Show(title, caption, confirmationText, messageContent, MsgBoxButton.YesNo, msgBoxImage);
            if (result == MsgBoxResult.Yes || result == MsgBoxResult.OK)
                dialogResult = true;
            else
                dialogResult = false;

            return dialogResult;

        }
    }
}
