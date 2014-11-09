using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using MingleView.UI.Service;

namespace MingleView.UI.ViewModel
{
    public class SendErrorReportViewModel : ViewModelBase
    {
        #region Fields
        RelayCommand _sendErrorReportCmd;
        RelayCommand _cancelCmd;
        string _userName;
        string _userEMail;
        string _subject;
        string _message;
        #endregion //Fields

        #region SendErrorReport Properties

        public string UserName
        {
            get { return _userName; }
            set
            {
                if (value == _userName)
                    return;

                _userName = value;

                base.OnPropertyChanged("UserName");
            }
        }
        public string UserEMail
        {
            get { return _userEMail; }
            set
            {
                if (value == _userEMail)
                    return;

                _userEMail = value;

                base.OnPropertyChanged("UserEMail");
            }
        }
        public string Subject
        {
            get { return _subject; }
            set
            {
                if (value == _subject)
                    return;

                _subject = value;

                base.OnPropertyChanged("Subject");
            }
        }
        public string Message
        {
            get { return _message; }
            set
            {
                if (value == _message)
                    return;

                _message = value;

                base.OnPropertyChanged("Message");
            }
        }
      
        #endregion //SendErrorReport Properties

        #region SendErrorReport Activities
        public ICommand SendErrorReportCmd
        {
            get
            {
                if (_sendErrorReportCmd == null)
                {
                    //_sendErrorReportCmd = new RelayCommand(param => SendErrorReport(this, new SendErrorReportViewModelEvntArgs(this)));
                    _sendErrorReportCmd = new RelayCommand(param => SendErrorReportInfo());
                }
                return _sendErrorReportCmd;
            }
        }
        public ICommand CancelCmd
        {
            get
            {
                if (_cancelCmd == null)
                {
                    _cancelCmd = new RelayCommand(param => OnRequestClose());
                }
                return _cancelCmd;
            }
        }
        #endregion //SendErrorReport Activities

        void SendErrorReportInfo()
        {
            //SendEmail();
            ErrorReporting mingleReport = new ErrorReporting();
            MessageBoxService msgui=new MessageBoxService(); 
            mingleReport.Send(UserName, UserEMail, Subject, Message);      
            msgui.Show(MingleView.Properties.Strings.SendErrorReportViewModel_Send_Success);
            OnRequestClose();
        }

        #region RequestClose [event]

        /// <summary>
        /// Raised when this workspace should be removed from the UI.
        /// </summary>
        public event EventHandler RequestClose;

        /// <summary>
        /// If requested to close and a RequestClose delegate has been set then call it.
        /// </summary>
        void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
        #endregion // RequestClose [event]
    }
}
