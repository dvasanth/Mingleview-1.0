using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace MingleView.UI.ViewModel
{
    public class JoinMeetingViewModel : ViewModelBase
    {
        #region Fields
        RelayCommand _joinMeetingCmd;
        RelayCommand _joinCancelCmd;
        string _ticketID;
        string _participantName;
        bool _showJoinMeeting;
        #endregion //Fields

        #region JoinMeeting Properties

        public string TicketID
        {
            get { return _ticketID; }
            set
            {
                if (value == _ticketID)
                    return;

                _ticketID = value;

                base.OnPropertyChanged("TicketID");
            }
        }
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

        public bool ShowJoinMeeting
        {
            get
            {
                return _showJoinMeeting;
            }
            set
            {
                if (value == _showJoinMeeting)
                    return;

                _showJoinMeeting = value;

                base.OnPropertyChanged("ShowJoinMeeting");
            }
        }
        #endregion //JoinMeeting Properties

        #region JoinMeeting Activities
        public ICommand JoinMeetingCmd
        {
            get
            {
                if (_joinMeetingCmd == null)
                {
                    _joinMeetingCmd = new RelayCommand(param => JoinMeeting(this, new JoinMeetingViewModelEvntArgs(this)));
                }
                return _joinMeetingCmd;
            }
        }

        public ICommand JoinCancelCmd
        {
            get
            {
                if (_joinCancelCmd == null)
                {
                    _joinCancelCmd = new RelayCommand(param => CancelJoinMeeting());
                }
                return _joinCancelCmd;
            }
        }
        void CancelJoinMeeting()
        {
            ShowJoinMeeting = false;
        }
        #endregion //JoinMeeting Activities

        #region Events
        public event EventHandler<JoinMeetingViewModelEvntArgs> JoinMeeting = delegate { };
        #endregion //Events
    }
}
