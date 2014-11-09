using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MingleView.Model;

namespace MingleView.Service
{
    public class RDInvitationEventArgs : EventArgs
    {
        public RDInvitation Invitation { get; set; }
    }
    public class ChatMessageEventArgs : EventArgs
    {
        public ChatMessage Message { get; set; }
    }
    public class ParticipantEventArgs : EventArgs
    {
        public Participant ParticipantInfo { get; set; }
    }
    public class MeetingStatusEventArgs : EventArgs
    {
        public string MeshID { get; set; }
        public string ErrorMsg { get; set; }

    }
}
