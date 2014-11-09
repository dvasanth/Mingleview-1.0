using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MingleView.Service;
namespace MingleView.Service
{
    public interface IMeetingEvents
    {
        event EventHandler<ParticipantEventArgs> ParticipantJoined;
        event EventHandler<ParticipantEventArgs> ParticipantUpdated;
        event EventHandler<ParticipantEventArgs> ParticipantDeparted;
        event EventHandler<ChatMessageEventArgs> ChatMessageReceived;
        event EventHandler<RDInvitationEventArgs> RDInvitationReceived;
        event EventHandler<MeetingStatusEventArgs> MeetingCreationStatusUpdate;
        event EventHandler<MeetingStatusEventArgs> MeetingJoinStatusUpdate;
        event EventHandler<MeetingStatusEventArgs> MeetingClosed;

    }
}
