using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF_Mvvm_and_EF.Services
{
    public class MessageDialogService : IMessageDialogService
    {
        public MessageDialogResult ShowOkCancelDialog(string text, string title)
        {

            var ret = MessageBox.Show(text, title, MessageBoxButton.OKCancel);
            return ret == MessageBoxResult.OK? 
                MessageDialogResult.Ok:MessageDialogResult.Cancel;
        }

        public void ShowInfoDialog(string text)
        {
            MessageBox.Show(text, "INFO");
        }
    }
    public enum MessageDialogResult
    {
        Ok,
        Cancel
    }


}
