using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace MingleView.UI.ViewModel
{
    public class QuickStartMeetingViewModel : ViewModelBase
    {
        #region Fields
        RelayCommand _QuickStartCmd;
        RelayCommand _quickStartCancelCmd;
        string _participantName;
        bool _showStartMeeting;
        #endregion //Fields

        #region QuickStart Properties
        public string ParticipantName
        {
            get { return _participantName; }
            set
            {
                if (value == _participantName)
                    return;

                _participantName = value;

                base.OnPropertyChanged("ParticipantName");
            }
        }

        public bool ShowStartMeeting
        {
            get
            {
                return _showStartMeeting;
            }
            set
            {
                if (value == _showStartMeeting)
                    return;

                _showStartMeeting = value;

                base.OnPropertyChanged("ShowStartMeeting");
            }
        }
        #endregion //QuickStart Properties

        #region QuickStart Activities
        public ICommand QuickStartCmd
        {
            get
            {
                if (_QuickStartCmd == null)
                {
                    _QuickStartCmd = new RelayCommand(param => StartMeeting(this, new QuickStartMeetingViewModelEvntArgs(this)));
                }
                return _QuickStartCmd;
            }
        }
        public ICommand QuickStartCancelCmd
        {
            get
            {
                if (_quickStartCancelCmd == null)
                {
                    _quickStartCancelCmd = new RelayCommand(param => CancelQuickStart());
                }
                return _quickStartCancelCmd;
            }
        }
        void CancelQuickStart()
        {
            ShowStartMeeting = false;
        }
        #endregion //QuickStart Activities

        #region Events
        public event EventHandler<QuickStartMeetingViewModelEvntArgs> StartMeeting = delegate { };
        #endregion //Events
    }
}
