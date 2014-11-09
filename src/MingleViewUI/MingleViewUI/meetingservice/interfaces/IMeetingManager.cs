﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using MingleView.Model;

namespace MingleView.Service
{
    public interface IMeetingManager
    {
        IOnlineStatus OnlineStatus { get; }
        IMeetingChannel InformAll { get; }
        IMeetingEvents MeetingEvents { get; }
        Participant Self { get; set; }
        string MeetingID { get; }
        void CreateNewMeeting(Participant meetingHost);
        void JoinMeeting(string sMeetingID, Participant viewParticipant);
        void LeaveMeeting();
        void ChangePresenter(Participant newPresenter);
    }
}
