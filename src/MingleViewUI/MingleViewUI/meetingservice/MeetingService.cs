using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Windows;
using p2p.mesh;
using MingleView.P2P.StateSync;
using System.Collections.ObjectModel;
using System.ServiceModel;
using MingleView.Model;

namespace MingleView.Service
{
    public class MeetingService : IMeetingManager,IMeshMeeting, IMeetingEvents  
    {
         //event notifiers
        public event EventHandler<ParticipantEventArgs> ParticipantJoined;
        public  event EventHandler<ParticipantEventArgs> ParticipantUpdated;
        public event EventHandler<ParticipantEventArgs> ParticipantDeparted;
        public event EventHandler<ChatMessageEventArgs> ChatMessageReceived;
        public event EventHandler<RDInvitationEventArgs> RDInvitationReceived;
        public event EventHandler<MeetingStatusEventArgs> MeetingCreationStatusUpdate;
        public event EventHandler<MeetingStatusEventArgs> MeetingJoinStatusUpdate;
        public event EventHandler<MeetingStatusEventArgs> MeetingClosed;
        //records
        private SynchronizedCollection<Participant> Participants { get; set; }
        private SynchronizedCollection<ChatMessage> ChatMessages { get; set; }
        private RDInvitation ScreenInvitation { get;  set; }

        public Participant Self { get; set; }
        //privates
        private P2PMesh<IMeetingChannel,MeetingService> _meetingMesh=new P2PMesh<IMeetingChannel,MeetingService>();
        private delegate string StartMeshDelegate(MeetingService thisRef, Participant host);
        private delegate void JoinMeshDelegate(MeetingService thisRef, Participant viewer, string sMeetingID);

        /**********************
         * constructor
         * **********************/
        public MeetingService()
        {
            Participants = new SynchronizedCollection<Participant>();
            ChatMessages = new SynchronizedCollection<ChatMessage>();
           
        }
        #region MeetingService Members
        public IMeetingChannel InformAll { get { return _meetingMesh.MeshChannel; } }
        public IMeetingEvents MeetingEvents { get { return this; } }
        public IOnlineStatus OnlineStatus { get { return _meetingMesh.MeshChannel.GetProperty<IOnlineStatus>(); } }
        public string MeetingID { get { return _meetingMesh.MeshID; } }  
        #endregion
        #region IMeshMeeting Members
        /**********************
         * information about the service & upgrade if require
         * *********************/
        public void VersionInfo(AppInfo appInfo)
        {
            if (appInfo > AutoUpdater.GetAppInfo())
            {
                AutoUpdater serviceUpgrade = new AutoUpdater();
                //the other participant version is higher do update
                serviceUpgrade.Update(appInfo);
            }
           
        }
        /*******************
         * new joined guys informs to other about his presence
         * *****************/
        public void NewParticipant(Participant newParticipant)
        {
           //add the participant 
            if (!Participants.Contains(newParticipant))
            {
                Logger.Trace(Logger.MessageType.Informational, "Received NewParticipant " + newParticipant.Name); 
                Participants.Add(newParticipant);
               
                EventHandler<ParticipantEventArgs> handler = this.ParticipantJoined;
                if (handler != null)
                    this.ParticipantJoined(this, new ParticipantEventArgs() { ParticipantInfo = newParticipant });
            }
        }
        /*********************
         * sometimes presenter or host is changed & requires participant field changes
         * ******************/
        public void UpdateParticipant(Participant updatedParticipant)
        {
            if (Participants.Contains(updatedParticipant))
            {
                Logger.Trace(Logger.MessageType.Informational, "Received UpdateParticipant " + updatedParticipant.Name);
                EventHandler<ParticipantEventArgs> handler = this.ParticipantUpdated;
                if (handler != null)
                    this.ParticipantUpdated(this, new ParticipantEventArgs() { ParticipantInfo = updatedParticipant });
            }

        }
        /*********************
       * Participant left
       * ******************/
        public void ParticipantDepart(Participant departedParticipant)
        {
            if (Participants.Contains(departedParticipant))
            {
                Logger.Trace(Logger.MessageType.Informational, "Received ParticipantDepart " + departedParticipant.Name);
                EventHandler<ParticipantEventArgs> handler = this.ParticipantDeparted;
                if (handler != null)
                    this.ParticipantDeparted(this, new ParticipantEventArgs() { ParticipantInfo = departedParticipant });
                Participants.Remove(departedParticipant); 
            }

        }
        /******************
         * receives chat message from others
         * *********************/
        public void Chat(ChatMessage msg)
        {
            //notify thro events
            if (!ChatMessages.Contains(msg))
            {
                Logger.Trace(Logger.MessageType.Informational, "Received Chat from  " + msg.Source.Name  );
                ChatMessages.Add(msg);  
                EventHandler<ChatMessageEventArgs> handler = this.ChatMessageReceived;
                if (handler != null)
                    this.ChatMessageReceived(this, new ChatMessageEventArgs() { Message = msg });
            }

        }
        /***********************
         * presenter providing screen access
         * ********************/
        public void AccessScreen(RDInvitation invitation)
        {
            if (ScreenInvitation == null || //we don't have invitation
                !ScreenInvitation.Equals(invitation) //or new invitation
                )
            {
                Logger.Trace(Logger.MessageType.Informational, "Received AccessScreen from  " + invitation.Inviter .Name);
                ScreenInvitation = invitation;
                //got a new invitation
                EventHandler<RDInvitationEventArgs> handler = this.RDInvitationReceived ;
                if (handler != null)
                    this.RDInvitationReceived(this, new RDInvitationEventArgs() { Invitation = invitation });
    
            }
        }

        #endregion
        #region IPeerRecordSync Members
        /***********************
         * this will reply all participants to the caller
         * ******************/
         public void RequestRecordIds(PeerRecordsIdRequest request)
         {
             PeerRecordsDetailReply detailReply = new PeerRecordsDetailReply(this.Self);
             List<PeerRecord> records=null ;

             if (request.Requester.Equals(this.Self ))
                 return;//ignore sync request from same m/c
             //version sync
             if (request.RecordType == PEER_RECORD_TYPE.VERINFO)
             {

                Logger.Trace(Logger.MessageType.Informational, "Syncing Request for PEER_RECORD_TYPE.VERINFO ");
                records = new List<PeerRecord>(); 
                records.Add(new VersionRecord( AutoUpdater.GetAppInfo()));
             }
             //participant 
             if (request.RecordType == PEER_RECORD_TYPE.PARTICIPANT)
             {
                                
                 if (Participants.Count > 0)
                 {
                     Logger.Trace(Logger.MessageType.Informational, "Syncing Request for PEER_RECORD_TYPE.PARTICIPANT with "+Participants.Count+ " Records" );
                     records =
                         (from part in Participants
                          select new ParticipantRecord(part)).Cast<PeerRecord>().ToList();
                 }
                 //new neighbour joined asking for sync
                 //OnNewParticipantJoined(_meetingMesh.CallbackChannel);
             }

             //chat message
             if (request.RecordType == PEER_RECORD_TYPE.CHAT_MSG)
             {
                 if (ChatMessages.Count > 0)
                 {
                     Logger.Trace(Logger.MessageType.Informational, "Syncing Request for PEER_RECORD_TYPE.CHAT_MSG with " + ChatMessages.Count + " Records");
                     records =
                         (from msg in ChatMessages
                          select new ChatRecord(msg)).Cast<PeerRecord>().ToList();
                 }
             }

             //rd invitation
             if (request.RecordType == PEER_RECORD_TYPE.RDINVITATION )
             {
                 //send if we have a invitation
                 if (ScreenInvitation != null )
                 {
                     Logger.Trace(Logger.MessageType.Informational, "Syncing Request for PEER_RECORD_TYPE.RDINVITATION with " + ScreenInvitation.Inviter.Name   + "screen invitation");
                     records = new List<PeerRecord>(); 
                     records.Add(new RDInvitationRecord(ScreenInvitation));
   
                 }
             }


             //reply back only if we have records
             if (records != null)
             {
                 detailReply.Records = new List<PeerRecord>(records);
                 _meetingMesh.CallbackChannel.RecordDetailsAck(detailReply);
             }
         }

        /*******
         * *******/
        public void RecordIdsReply(PeerRecordsIdsReply reply)
        {
        }
        /********
         * ***********/
        public void RequestRecordDetails(List<Guid> RecordIds)
        {
        }
        /*****************
         * receiving records from neighbour
         * ************/
        public void RecordDetailsAck(PeerRecordsDetailReply PeerRecords)
        {
            if (PeerRecords.Records.Count == 0 ||
                PeerRecords.ReplyFrom.Equals(this.Self ))
            {
                return;
            }
            //check if VerInfo record then validate verinfo
            if (PeerRecords.Records[0] is VersionRecord)
            {
                AppInfo appInfo = (PeerRecords.Records[0] as VersionRecord).VerInfo;
                if (appInfo > AutoUpdater.GetAppInfo())
                {
                    AutoUpdater serviceUpgrade = new AutoUpdater();
                    //the other participant version is higher do update
                    serviceUpgrade.Update(appInfo);
                    return; 
                }
                //is he is lower verion then tell him to upgrade
                if (appInfo < AutoUpdater.GetAppInfo())
                {
                    _meetingMesh.CallbackChannel.VersionInfo(AutoUpdater.GetAppInfo());
                }
                onJoiningMeshComplete();
            }
            //check if participant record then sync with ours
            if (PeerRecords.Records[0] is ParticipantRecord)
            {
                List<Participant>  receivedParticipants = new List<Participant> (
                 (from record in  PeerRecords.Records 
                  select new Participant((record as ParticipantRecord).ParticipantInfo )).ToList());
                Logger.Trace(Logger.MessageType.Informational, "Participant Records received :" + receivedParticipants.Count);
                foreach(Participant participant in receivedParticipants)
                {
                    NewParticipant(participant);
                }
                
            }

            //check if chat records
            if (PeerRecords.Records[0] is ChatRecord)
            {
                List<ChatMessage> receivedMsg = new List<ChatMessage>(
                 (from record in PeerRecords.Records
                  select new ChatMessage((record as ChatRecord).Message)).ToList());
                Logger.Trace(Logger.MessageType.Informational, "Chat Records received :" + receivedMsg.Count);
                foreach (ChatMessage msg in receivedMsg)
                {
                    Chat(msg);
                }
             }

            //check if rdinvitation
            if (PeerRecords.Records[0] is RDInvitationRecord )
            {
                Logger.Trace(Logger.MessageType.Informational, "Syncing RDInvitation Record received from " + (PeerRecords.Records[0] as RDInvitationRecord).Invitation.Inviter.Name );
                AccessScreen((PeerRecords.Records[0] as RDInvitationRecord).Invitation ); 
            }

        }

        #endregion
      
        /******************************************
         * returns a meeting id with which the other user can join the meeting
         * *******/
        public void CreateNewMeeting(Participant meetingHost)
        {
            StartMeshDelegate thread =new StartMeshDelegate(startMesh);

            this.Self = meetingHost;
            thread.BeginInvoke(this, meetingHost, new AsyncCallback(meshCreationStatusCallback), this);   
        }
        /************
         * thread to create mesh operation
         * *************/
        static string startMesh(MeetingService thisRef, Participant host)
        {
            Logger.Trace(Logger.MessageType.Informational, "startMesh delegate:" + host.Name );
            thisRef._meetingMesh.Start(thisRef);

             return thisRef._meetingMesh.MeshID;
        }
        /************
        * callback for mesh operation completed
        * *********/
        static void meshCreationStatusCallback(IAsyncResult result)
        {
            //AsyncResult 
            System.Runtime.Remoting.Messaging.AsyncResult aResult = (System.Runtime.Remoting.Messaging.AsyncResult)result;
            StartMeshDelegate method = (StartMeshDelegate)aResult.AsyncDelegate;
            MeetingService thisRef = (MeetingService)aResult.AsyncState;
            string sMeshID;
            EventHandler<MeetingStatusEventArgs> handlerMeshStatusUpdate = thisRef.MeetingClosed;

            //try & catch exception if any
            try
            {
                sMeshID = method.EndInvoke(aResult);
            }
            catch (MingleViewException me)
            {
                Logger.Trace(Logger.MessageType.Failure , "Caught exception in startMesh : " + me.Message );
                //notify the callers error status

                if (handlerMeshStatusUpdate != null)
                    thisRef.MeetingClosed(thisRef, new MeetingStatusEventArgs() { ErrorMsg = me.Message });
                return;
            }
           //REGISTER offline handler
            thisRef.OnlineStatus.Offline += new EventHandler(thisRef.onMeshChannelOffline);  

            //notify the callers the status
            if (handlerMeshStatusUpdate != null)
                thisRef.MeetingCreationStatusUpdate(thisRef, new MeetingStatusEventArgs() { MeshID = sMeshID });

            //post completion operation
            //inform abt you to all
            thisRef.InformAll.NewParticipant(thisRef.Self);
 
           }
        /****************************
           * join meeting with meeting id
           * *******************/
        public void JoinMeeting(string sMeetingID, Participant viewParticipant)
        {
            JoinMeshDelegate thread = new JoinMeshDelegate(joinMesh);
            Logger.Trace(Logger.MessageType.Informational, "Starting join meeting for " + viewParticipant.Name  );
            this.Self = viewParticipant;
            thread.BeginInvoke(this, viewParticipant,sMeetingID, new AsyncCallback(meshJoinStatusCallback), this);   
           
        }
        /************
         * thread to do join mesh operation
        * *************/
        static void joinMesh(MeetingService thisRef, Participant viewer, string sMeetingID)
        {
            thisRef._meetingMesh.JoinMesh(thisRef, sMeetingID);
        }
         /************
        * gets invoked after syncing all participant list from neighbouring peers
         * we are joining the mesh & this func gets invoked after syncing completed
        * *************/
         private void onJoiningMeshComplete()
        {
            Logger.Trace(Logger.MessageType.Informational, "onJoiningMeshComplete");
            InformAll.RequestRecordIds(new PeerRecordsIdRequest(PEER_RECORD_TYPE.CHAT_MSG,Self ));
            InformAll.RequestRecordIds(new PeerRecordsIdRequest(PEER_RECORD_TYPE.PARTICIPANT, Self));
            InformAll.RequestRecordIds(new PeerRecordsIdRequest(PEER_RECORD_TYPE.RDINVITATION, Self));
            //inform abt you to all
            InformAll.NewParticipant(this.Self);

        }
        /************
         * callback for join mesh operation completed
         * *********/
        static void meshJoinStatusCallback(IAsyncResult result)
        {
            System.Runtime.Remoting.Messaging.AsyncResult aResult = (System.Runtime.Remoting.Messaging.AsyncResult)result;
            MeetingService thisRef = (MeetingService)aResult.AsyncState;
            JoinMeshDelegate method = (JoinMeshDelegate)aResult.AsyncDelegate;
            EventHandler<MeetingStatusEventArgs> handlerMeshJoinStatusUpdate = thisRef.MeetingJoinStatusUpdate;

            //try & catch exception if any
            try
            {
                method.EndInvoke(aResult);
            }
            catch (MingleViewException mi)
            {
                Logger.Trace(Logger.MessageType.Failure, "Caught exception in joinMesh : " + mi.Message);
                EventHandler<MeetingStatusEventArgs> handler = thisRef.MeetingClosed;

                if (handlerMeshJoinStatusUpdate != null)
                           thisRef.MeetingClosed(thisRef,new MeetingStatusEventArgs(){ ErrorMsg = mi.Message});
                 return;
            }


            //sucesss
            if (handlerMeshJoinStatusUpdate != null)
                thisRef.MeetingJoinStatusUpdate(thisRef, new MeetingStatusEventArgs() { });
   
          

            //sync data with others
            thisRef.InformAll.RequestRecordIds(new PeerRecordsIdRequest(PEER_RECORD_TYPE.VERINFO, thisRef.Self ));

    
          }

            /**********************
             * Close meeting
             * ********************/
        public void LeaveMeeting()
        {
            if(_meetingMesh.MeshChannel != null)
                            InformAll.ParticipantDepart(this.Self);
            _meetingMesh.Close();  
        }
        /***********************
         * mesh status is set to online--
         * offline if no neigbour peer is connected
         * ********************/
        void onMeshChannelOffline(object sender, EventArgs e)
        {
            EventHandler<MeetingStatusEventArgs> meshDownUpdate = this.MeetingClosed;
         
       //    if (meshDownUpdate != null)
       //        this.MeetingClosed(this, new MeetingStatusEventArgs() { });
          
        }
   

        /*************************
         * changes the presenter 
         * resets the exisiting & sets the new participant
         * ************************/
        public void ChangePresenter(Participant newPresenter)
        {
            Participant oldPresenter = null;
            lock(Participants.SyncRoot )
            {
                foreach(Participant  user in Participants) 
                {
                    //find the old presenter
                    if (user.IsPresenter())
                    {
                        //reset the flag
                        user.ChangeToViewer();
                        oldPresenter = user;
                        break;
                    }
                }
            }//lock

            //update the old presenter
            if(oldPresenter != null)
                this.InformAll.UpdateParticipant(oldPresenter);
            newPresenter.ChangeToPresenter();  
            //update the new presenter
            this.InformAll.UpdateParticipant(newPresenter);   
        }
    }
}
