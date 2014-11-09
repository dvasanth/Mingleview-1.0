using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using MingleView.Properties;
namespace MingleView.UI.Service
{
    public class MessageBoxService
    {
        string _defaultCaption;
        public MessageBoxService()
        {
            _defaultCaption = Strings.MainWindowViewModel_DisplayName; 
        }
        public GenericMessageBoxResult Show(string message, string caption, GenericMessageBoxButton buttons)
        {
            MessageBoxButton slButtons = MessageBoxButton.OK;
            if (buttons == GenericMessageBoxButton.Ok)
            {
                slButtons = MessageBoxButton.OK;
            }
            if (buttons == GenericMessageBoxButton.OkCancel)
            {
                slButtons = MessageBoxButton.OKCancel;
            }
            if (buttons == GenericMessageBoxButton.YesNo)
            {
                slButtons = MessageBoxButton.YesNo;   
            }
            var result = MessageBox.Show(message, caption, slButtons,MessageBoxImage.Stop );

            if (result == MessageBoxResult.OK)
            {
                return GenericMessageBoxResult.Ok; 
            }
            if (result == MessageBoxResult.Cancel)
            {
                return GenericMessageBoxResult.Cancel;
            }
            if(result == MessageBoxResult.Yes)
            {
                return GenericMessageBoxResult.Yes; 
            }
            if(result == MessageBoxResult.No)
            {
                return GenericMessageBoxResult.No;  
            }
            return  GenericMessageBoxResult.Cancel;
        }

        public void Show(string message, string caption)
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK);
        }

        public void Show(string message)
        {
            MessageBox.Show(message, _defaultCaption, MessageBoxButton.OK);
        }
    }
}
