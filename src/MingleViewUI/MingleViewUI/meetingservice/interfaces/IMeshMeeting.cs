using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using MingleView.Service;
using MingleView.P2P.StateSync;
using MingleView.Model;
namespace MingleView.Service
{
    [ServiceContract(Namespace = "http://mingleview.crow-soft.com/meeting",
       CallbackContract = typeof(IMeshMeeting))]
    public interface IMeshMeeting //: IPeerRecordSync
    {
        //serviceInfo    
        [OperationContract(IsOneWay = true)]
        void VersionInfo(AppInfo appInfo);

    
        //new guy tells abt him to others
        [OperationContract(IsOneWay = true)]
        void NewParticipant(Participant newParticipant);

        //change the partipant acccess levels most prob by the host
        [OperationContract(IsOneWay = true)]
        void UpdateParticipant(Participant updatedParticipant);

        //change the partipant acccess levels most prob by the host
        [OperationContract(IsOneWay = true)]
        void ParticipantDepart(Participant departedParticipant);
   
        // Synchronization of Transactions
        [OperationContract(IsOneWay = true)]
        void RequestRecordIds(PeerRecordsIdRequest request);

        [OperationContract(IsOneWay = true)]
        void RecordIdsReply(PeerRecordsIdsReply reply);

        [OperationContract(IsOneWay = true)]
        void RequestRecordDetails(List<Guid> RecordIds);

        [OperationContract(IsOneWay = true)]
        void RecordDetailsAck(PeerRecordsDetailReply PeerRecords);

        //chat related
        [OperationContract(IsOneWay = true)]
        void Chat(ChatMessage msg);
        
        //remote desktop related
        [OperationContract(IsOneWay = true)]
        void AccessScreen(RDInvitation invitation);
    }
    public interface IMeetingChannel : IMeshMeeting, IClientChannel
    {
    }
}
