using System;
using System.Windows;

using CompassPlus.Globalization;


namespace CompassPlus.Services
{
    public interface IMessageService
    {
        void ShowMessage( string message, string caption);
        bool ShowMessageOkCancel(string message, string caption);
    }

    class MessageService : IMessageService
    {
        public void ShowMessage(string message, string caption)
        {
            ShowMessageInternal(message, caption, MessageBoxButton.OK);
        }

        public bool ShowMessageOkCancel(string message, string caption)
        {
            return ShowMessageInternal(message, caption, MessageBoxButton.OKCancel);
        }

        private bool ShowMessageInternal(string message, string caption, MessageBoxButton button)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException("message");
            }
            if (string.IsNullOrWhiteSpace(caption))
            {
                caption = new LocalizedStrings()[Constants.Name];
            }
            var result = MessageBox.Show(message, caption, button);
            if (result == MessageBoxResult.Cancel)
            {
                return false;
            }
            return true;
        }
    }
}
