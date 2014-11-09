using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Forms;
using MingleView.Model;
using MingleView.Service;
using System.Windows.Threading;

namespace MingleView.UI.ViewModel
{
    public class RemoteScreenViewModel : MeetingWorkspaceViewModel
    {
        #region enums

        #endregion
        #region Fields

        readonly IMeetingManager _meetingManager;
        private readonly Dispatcher _dispatcher;
         RDInvitation _rdpInvitation=null;
        #endregion //Fields
        #region Constructor
        public RemoteScreenViewModel(IMeetingManager meetingManager, Dispatcher dispatcher)
        {
            _meetingManager = meetingManager;
            _dispatcher = dispatcher;
            //register the event handlers
            _meetingManager.MeetingEvents.RDInvitationReceived += delegate(object sender, RDInvitationEventArgs invitationRgs)
            {
                this._dispatcher.Invoke(
                    (Action)delegate
                    {
                        OnRDPInvitationReceive(invitationRgs.Invitation);
                    },
                    null);
            };
        }
        /***
         * set the invitation to be used for the activex control
         * ********/
        public RDInvitation ScreenInvitation
        {
            get { return _rdpInvitation; }
            set
            {
                if (value == _rdpInvitation)
                    return;

                _rdpInvitation = value;

                base.OnPropertyChanged("ScreenInvitation");
            }
        }
        /******************
         * identity in dekstop session with participant
         * this associates desktop attendees with participant in meetingsession
         * * *****************/
        public string RDAttendeeName
        {
            get { return _meetingManager.Self.Id; }
   
        }
  
        #endregion

        #region Presentation Properties
 
 
        /********************
        * FitToScreenCmd 
        * ****************/
        //public ICommand FitToScreenCmd
        //{
        //    get
        //    {
        //        if (_fitToScreenCmd == null)
        //        {
        //            _fitToScreenCmd = new RelayCommand(param => this.FitToScreen());
        //        }
        //        return _fitToScreenCmd; 
        //    }

        //}
        
        #endregion
        #region event notification
        /************
         * invitationreceived uses this to update activex control
         * ************/
        private void OnRDPInvitationReceive(RDInvitation invitation)
        {
            //received invitation
            if(!invitation.Inviter.Equals(  this._meetingManager.Self))  //don't try to open our screen invitation
                    ScreenInvitation = invitation; 
           
        }

        private void FitToScreen()
        {
        }
        #endregion
    }
}

