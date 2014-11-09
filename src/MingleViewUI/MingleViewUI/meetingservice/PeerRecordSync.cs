using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using MingleView.Service;
using System.Runtime.Serialization;
using MingleView.Model;

namespace MingleView.P2P.StateSync
{
    //decides how much user can control a participant
    public static class PEER_RECORD_TYPE
    {
        public const int VERINFO = 1;
        public const int PARTICIPANT = 2;
        public const int CHAT_MSG = 3;
        public const int RDINVITATION = 4;
    }
  

    [MessageContract]
    public class PeerRecordsIdRequest
    {
       [MessageBodyMember]
       public int RecordType;

       [MessageBodyMember]
       public Participant Requester;

       [PeerHopCount]
       public int Hops;

       public PeerRecordsIdRequest()
       {
           Hops = 1;
       }
        public PeerRecordsIdRequest(int recordType,Participant requester)
        {
            Hops = 1;
            RecordType = recordType;
            Requester = requester; 
        }
    }

    // Crucial point: We don't need the the hop count control here
    // because we are replying on the callback channel, which ONLY
    // responds to the person that sent us the original message....
    // In short, in response to a 1-hop request, the code will reply
    // only to the requester...
    [MessageContract]
    public class PeerRecordsIdsReply
    {
        [MessageBodyMember]
        public Participant  ReplyFrom;

        [MessageBodyMember]
        public List<Guid> RecordIds;
    }

    //base class of the record, custom record can be added to this
    //Need to inherit this base to override store custom data record
    //need to inform in advance abt derived types passed
    [MessageContract]
    [KnownType(typeof(VersionRecord))]
    [KnownType(typeof(ParticipantRecord))]
    [KnownType(typeof(ChatRecord))]
    [KnownType(typeof(RDInvitationRecord))]
    public class PeerRecord
    {
    }
    //version records
    [MessageContract]
    public class VersionRecord : PeerRecord
    {
        [MessageBodyMember]
        public AppInfo VerInfo;
        public VersionRecord()
        {
        }
        public VersionRecord(AppInfo verInfo)
        {
            VerInfo = verInfo;
        }

    }
    //participant records
    [MessageContract]
    public class ParticipantRecord : PeerRecord
    {
        [MessageBodyMember]
        public Participant ParticipantInfo;
        public ParticipantRecord()
        {
        }
        public ParticipantRecord(Participant participant)
        {
            ParticipantInfo = participant;
        }

    }
    //chat records
    [MessageContract]
    public class ChatRecord : PeerRecord
    {
        [MessageBodyMember]
        public ChatMessage Message;
        public ChatRecord()
        {
        }
        public ChatRecord(ChatMessage message)
        {
            Message = message;
            
        }

    }
    //invitation record
    [MessageContract]
    public class RDInvitationRecord : PeerRecord
    {
        [MessageBodyMember]
        public RDInvitation Invitation;
        public RDInvitationRecord()
        {
        }
        public RDInvitationRecord(RDInvitation invitation)
        {
            Invitation = invitation;

        }

    }


    [MessageContract]
    public class PeerRecordsDetailReply
    {
        [MessageBodyMember]
        public Participant ReplyFrom;

        [MessageBodyMember]
        public List<PeerRecord> Records;
         public PeerRecordsDetailReply()
        {
        }
         public PeerRecordsDetailReply(Participant replyFrom)
        {
            ReplyFrom = replyFrom;

        }
    }





}
