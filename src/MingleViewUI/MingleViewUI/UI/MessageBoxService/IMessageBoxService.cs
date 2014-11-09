using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MingleView.UI.Service
{
    public enum GenericMessageBoxButton
    {
        Ok,
        OkCancel,
        YesNo
    }

    public enum GenericMessageBoxResult
    {
        Ok,
        Cancel,
        Yes,
        No
    }

    public interface IMessageBoxService
    {
        GenericMessageBoxResult Show(string message, string caption, GenericMessageBoxButton buttons);
        void Show(string message, string caption);
    }
}
